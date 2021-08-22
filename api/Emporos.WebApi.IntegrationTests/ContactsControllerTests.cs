using System;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Emporos.Core.Domain;
using Emporos.Core.Dto;
using FluentAssertions;
using Emporos.Core.Stores;
using Emporos.WebApi.IntegrationTests.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace Emporos.WebApi.IntegrationTests
{
    public class ContactsControllerTests : IClassFixture<ApplicationFactory>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly Fixture _fixture = new();

        public ContactsControllerTests(ApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Should_Get_ReturnContactsCorrectly_WhenTheContactsAreAvailable()
        {
            // Arrange
            var client = _factory.CreateClient();
            var contact = _fixture.Create<Contact>();
            var context = _factory.Server.Services.GetRequiredService<ApplicationDbContext>();
            await context.AddAsync(contact);
            await context.SaveChangesAsync();

            // Act
            var response = await client.GetAsync($"api/contacts");

            // Assert
            var actual = await response.ValidateAndReadContentAsync<ContactDto[]>();
            actual.Should().HaveCountGreaterThan(1).And.Contain(c => c.ContactId == contact.ContactId);
        }        
        
        [Fact]
        public async Task Should_Get_ReturnContactCorrectly_WhenTheContactIsAvailable()
        {
            // Arrange
            var client = _factory.CreateClient();
            var contact = _fixture.Create<Contact>();
            var context = _factory.Server.Services.GetRequiredService<ApplicationDbContext>();
            await context.AddAsync(contact);
            await context.SaveChangesAsync();

            // Act
            var response = await client.GetAsync($"api/contacts/{contact.ContactId}");

            // Assert
            var actual = await response.ValidateAndReadContentAsync<ContactDto>();
            actual.Should().BeEquivalentTo(contact);
        }  
        
        [Fact]
        public async Task Should_Get_ReturnNotFound_WhenTheContactIsNotAvailable()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync($"api/contacts/{Guid.NewGuid()}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_Post_CreateAndReturnNewContactCorrectly_WhenThePayloadIsValid()
        {
            // Arrange
            var client = _factory.CreateClient();
            var request = new ContactRequestDto
            {
                Email = "one@two.com",
                DateOfBirth = DateTime.Today.AddDays(-1),
                FirstName = "Name",
                Surname = "Surname"
            };
            
            var context = _factory.Server.Services.GetRequiredService<ApplicationDbContext>();
            var httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8,
                MediaTypeNames.Application.Json);

            // Act
            var response = await client.PostAsync("api/contacts", httpContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var actual = await response.ValidateAndReadContentAsync<ContactDto>();
            var persisted = await context.Contacts.FindAsync(actual.ContactId);

            request.Should().BeEquivalentTo(actual, s => s.Excluding(c => c.ContactId));
            request.Should().BeEquivalentTo(persisted, s => s.Excluding(c => c.ContactId));
        }        
        
        [Fact]
        public async Task Should_Post_ReturnBadRequest_WhenThePayloadIsInvalid()
        {
            // Arrange
            var client = _factory.CreateClient();
            var request = new ContactRequestDto
            {
                Email = "one",
                DateOfBirth = DateTime.Today.AddDays(1),
                FirstName = "Name".PadRight(101, '$'),
                Surname = "Surname".PadRight(101, '$'),
            };
            
            var context = _factory.Server.Services.GetRequiredService<ApplicationDbContext>();
            var httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8,
                MediaTypeNames.Application.Json);

            // Act
            var response = await client.PostAsync("api/contacts", httpContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var actual = await response.ReadValidationErrorContentAsync();
            actual.Should().Contain("\"title\":\"One or more validation errors occurred.\"");
            actual.Should().Contain("\"Email\":[\"'Email' is not a valid email address.\"]");
            actual.Should().Contain("\"Surname\":[\"The length of 'Surname' must be 100 characters or fewer");
            actual.Should().Contain("\"FirstName\":[\"The length of 'First Name' must be 100 characters or fewer. You entered 101 characters.\"]");
            actual.Should().Contain("\"DateOfBirth\":[\"The specified condition was not met for 'Date Of Birth'.\"]");
        }

        [Fact]
        public async Task Should_Put_UpdateAndReturnUpdatedContactCorrectly_WhenThePayloadIsValidAndContactExists()
        {
            // Arrange
            var client = _factory.CreateClient();
            var contact = _fixture.Create<Contact>();
            var request = new ContactRequestDto
            {
                Email = "one@two.com",
                DateOfBirth = DateTime.Today.AddDays(-1),
                FirstName = "Name",
                Surname = "Surname"
            };
            
            var context = _factory.Server.Services.GetRequiredService<ApplicationDbContext>();
            await context.AddAsync(contact);
            await context.SaveChangesAsync();
            
            var httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8,
                MediaTypeNames.Application.Json);

            // Act
            var response = await client.PutAsync($"api/contacts/{contact.ContactId}", httpContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var actual = await response.ValidateAndReadContentAsync<ContactDto>();
            var persisted = await context.Contacts.FindAsync(actual.ContactId);

            request.Should().BeEquivalentTo(actual, s => s.Excluding(c => c.ContactId));
            request.Should().BeEquivalentTo(persisted, s => s.Excluding(c => c.ContactId));
        }

        [Fact]
        public async Task Should_Put_ReturnsNotFound_WhenContactIsNotExist()
        {
            // Arrange
            var client = _factory.CreateClient();
            var request = new ContactRequestDto
            {
                Email = "one@two.com",
                DateOfBirth = DateTime.Today.AddDays(-1),
                FirstName = "Name",
                Surname = "Surname"
            };
            var httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8,
                MediaTypeNames.Application.Json);

            // Act
            var response = await client.PutAsync($"api/contacts/{Guid.NewGuid()}", httpContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}