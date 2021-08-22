using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using Emporos.Core.Domain;
using Emporos.Core.Dto;
using Emporos.Core.Handlers;
using Emporos.Core.Stores;
using EntityFrameworkCore.Testing.NSubstitute;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Emporos.Core.UnitTests.Handlers
{
    public class GetContactListRequestHandlerTests
    {
        private readonly Fixture _fixture = new();

        [Fact]
        public async Task Should_Handle_ReturnTheContactList_WhenTheListIsAvailable()
        {
            // Arrange
            var contacts = _fixture.CreateMany<Contact>(1).ToList();
            var dbContext = Create.MockedDbContextFor<ApplicationDbContext>();
            dbContext.Contacts.AddRange(contacts);
            await dbContext.SaveChangesAsync();
            var logger = Substitute.For<ILogger<GetContactListRequestHandler>>();
            var mapper = Substitute.For<IMapper>();
            mapper.Map<IEnumerable<ContactDto>>(Arg.Any<IEnumerable<Contact>>()).Returns(new[]
            {
                new ContactDto
                {
                    ContactId = contacts[0].ContactId,
                    FirstName = contacts[0].FirstName,
                    Surname = contacts[0].Surname,
                    DateOfBirth = contacts[0].DateOfBirth,
                    Email = contacts[0].Email,
                }
            });
            var sut = new GetContactListRequestHandler(dbContext, mapper, logger);

            // Act
            var actual = await sut.Handle(new GetContactListRequest(),
                CancellationToken.None);

            // Assert
            actual.Succeed.Should().BeTrue();
            actual.Result.Should().HaveCount(1).And.Contain(m => m.ContactId == contacts[0].ContactId);
        }

        [Fact]
        public async Task Should_Handle_ReturnErrorWithGenericErrorReason_WhenExceptionHappened()
        {
            // Arrange
            var contacts = _fixture.CreateMany<Contact>(1).ToList();
            var dbContext = Create.MockedDbContextFor<ApplicationDbContext>();
            dbContext.Contacts.Throws(new Exception());
            var logger = Substitute.For<ILogger<GetContactListRequestHandler>>();
            var mapper = Substitute.For<IMapper>();
            var sut = new GetContactListRequestHandler(dbContext, mapper, logger);

            // Act
            var actual = await sut.Handle(new GetContactListRequest(), CancellationToken.None);

            // Assert
            actual.Failed.Should().BeTrue();
            actual.ErrorReason.Should().Be(OperationErrorReason.GenericError);
        }
    }
    
    public class GetContactRequestHandlerTests
    {
        private readonly Fixture _fixture = new();

        [Fact]
        public async Task Should_Handle_ReturnTheContact_WhenItIsAvailable()
        {
            // Arrange
            var contacts = _fixture.CreateMany<Contact>(1).ToList();
            var dbContext = Create.MockedDbContextFor<ApplicationDbContext>();
            dbContext.Contacts.AddRange(contacts);
            await dbContext.SaveChangesAsync();
            var logger = Substitute.For<ILogger<GetContactRequestHandler>>();
            var mapper = Substitute.For<IMapper>();
            mapper.Map<ContactDto>(Arg.Any<Contact>()).Returns(new ContactDto
                {
                    ContactId = contacts[0].ContactId,
                    FirstName = contacts[0].FirstName,
                    Surname = contacts[0].Surname,
                    DateOfBirth = contacts[0].DateOfBirth,
                    Email = contacts[0].Email,
                }
            );
            var sut = new GetContactRequestHandler(dbContext, mapper, logger);

            // Act
            var actual = await sut.Handle(new GetContactRequest(contacts[0].ContactId),
                CancellationToken.None);

            // Assert
            actual.Succeed.Should().BeTrue();
            actual.Result.Should().BeEquivalentTo(contacts[0]);
        }

        [Fact]
        public async Task Should_Handle_ReturnErrorWithResourceNotFoundReason_WhenCanNotFindTheContact()
        {
            // Arrange
            var contacts = _fixture.CreateMany<Contact>(1).ToList();
            var dbContext = Create.MockedDbContextFor<ApplicationDbContext>();
            dbContext.Contacts.AddRange(contacts);
            await dbContext.SaveChangesAsync();
            var logger = Substitute.For<ILogger<GetContactRequestHandler>>();
            var mapper = Substitute.For<IMapper>();
            var sut = new GetContactRequestHandler(dbContext, mapper, logger);

            // Act
            var actual = await sut.Handle(new GetContactRequest(Guid.NewGuid()), CancellationToken.None);

            // Assert
            actual.Failed.Should().BeTrue();
            actual.ErrorReason.Should().Be(OperationErrorReason.ResourceNotFound);
        }
    }
}