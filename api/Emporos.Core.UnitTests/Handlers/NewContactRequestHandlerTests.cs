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
    public class NewContactRequestHandlerTests
    {
        private readonly Fixture _fixture = new();

        [Fact]
        public async Task Should_Handle_ReturnTheSuccessAndCreatedContact_WhenTheContactIsValid()
        {
            // Arrange
            var contact = _fixture.Create<ContactRequestDto>();
            var dbContext = Create.MockedDbContextFor<ApplicationDbContext>();
            var logger = Substitute.For<ILogger<NewContactRequestHandler>>();
            var mapper = Substitute.For<IMapper>();
            mapper.Map<ContactDto>(Arg.Any<Contact>()).Returns(
                new ContactDto
                {
                    FirstName = contact.FirstName,
                    Surname = contact.Surname,
                    DateOfBirth = contact.DateOfBirth,
                    Email = contact.Email,
                }
            );
            var sut = new NewContactRequestHandler(dbContext, mapper, logger);
            var newContactRequest = _fixture.Create<NewContactRequest>();

            // Act
            var actual = await sut.Handle(newContactRequest, CancellationToken.None);

            // Assert
            actual.Succeed.Should().BeTrue();
            actual.Result.Should().NotBeNull();

            dbContext.Contacts.Count().Should().Be(1);
        }

        [Fact]
        public async Task Should_Handle_ReturnError_WhenExceptionHappened()
        {
            // Arrange
            var dbContext = Create.MockedDbContextFor<ApplicationDbContext>();
            dbContext.Contacts.Throws(new Exception());
            var logger = Substitute.For<ILogger<NewContactRequestHandler>>();
            var mapper = Substitute.For<IMapper>();
            
            var sut = new NewContactRequestHandler(dbContext, mapper, logger);
            var newContactRequest = _fixture.Create<NewContactRequest>();

            // Act
            var actual = await sut.Handle(newContactRequest, CancellationToken.None);

            // Assert
            actual.Failed.Should().BeTrue();
            actual.ErrorReason.Should().Be(OperationErrorReason.GenericError);
        }
    }
}