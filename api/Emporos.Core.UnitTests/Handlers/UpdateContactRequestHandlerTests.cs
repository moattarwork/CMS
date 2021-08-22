using System;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Emporos.Core.UnitTests.Handlers
{
    public class UpdateContactRequestHandlerTests
    {
        private readonly Fixture _fixture = new();

        [Fact]
        public async Task Should_Handle_ReturnTheSuccess_WhenTheContactIdIsExist()
        {
            // Arrange
            var contacts = _fixture.CreateMany<Contact>(2).ToList();
            var dbContext = Create.MockedDbContextFor<ApplicationDbContext>();
            await dbContext.Contacts.AddRangeAsync(contacts);
            await dbContext.SaveChangesAsync();
            var logger = Substitute.For<ILogger<UpdateContactRequestHandler>>();
            var mapper = Substitute.For<IMapper>();
            mapper.Map<ContactDto>(Arg.Any<Contact>()).Returns(
                new ContactDto
                {
                    ContactId = contacts[0].ContactId,
                    FirstName = contacts[0].FirstName,
                    Surname = contacts[0].Surname,
                    DateOfBirth = contacts[0].DateOfBirth,
                    Email = contacts[0].Email,
                }
            );
            var sut = new UpdateContactRequestHandler(dbContext, mapper, logger);
            var contactRequest = _fixture.Create<ContactRequestDto>();

            // Act
            var actual = await sut.Handle(new UpdateContactRequest(contacts[0].ContactId, contactRequest),
                CancellationToken.None);

            // Assert
            actual.Succeed.Should().BeTrue();
            actual.Result.Should().NotBeNull();

            var updated = await dbContext.Contacts.FirstOrDefaultAsync(m => m.ContactId == contacts[0].ContactId);
            updated.ContactId.Should().Be(contacts[0].ContactId);
            updated.FirstName.Should().Be(contactRequest.FirstName);
            updated.Surname.Should().Be(contactRequest.Surname);
            updated.DateOfBirth.Should().Be(contactRequest.DateOfBirth);
            updated.Email.Should().Be(contactRequest.Email);
        }

        [Fact]
        public async Task Should_Handle_ReturnErrorWithResourceNotFoundReason_WhenTheContactIdIsNotExist()
        {
            // Arrange
            var dbContext = Create.MockedDbContextFor<ApplicationDbContext>();
            var logger = Substitute.For<ILogger<UpdateContactRequestHandler>>();
            var mapper = Substitute.For<IMapper>();
            var sut = new UpdateContactRequestHandler(dbContext, mapper, logger);
            var contactRequest = _fixture.Create<ContactRequestDto>();

            // Act
            var actual = await sut.Handle(new UpdateContactRequest(Guid.NewGuid(), contactRequest),
                CancellationToken.None);

            // Assert
            actual.Failed.Should().BeTrue();
            actual.ErrorReason.Should().Be(OperationErrorReason.ResourceNotFound);
        }

        [Fact]
        public async Task Should_Handle_ReturnErrorWithGenericErrorReason_WhenExceptionHappened()
        {
            // Arrange
            var dbContext = Create.MockedDbContextFor<ApplicationDbContext>();
            dbContext.Contacts.Throws(new Exception());
            var logger = Substitute.For<ILogger<UpdateContactRequestHandler>>();
            var mapper = Substitute.For<IMapper>();
            var sut = new UpdateContactRequestHandler(dbContext, mapper, logger);
            var contactRequest = _fixture.Create<ContactRequestDto>();

            // Act
            var actual = await sut.Handle(new UpdateContactRequest(Guid.NewGuid(), contactRequest),
                CancellationToken.None);

            // Assert
            actual.Failed.Should().BeTrue();
            actual.ErrorReason.Should().Be(OperationErrorReason.GenericError);
        }
    }
}