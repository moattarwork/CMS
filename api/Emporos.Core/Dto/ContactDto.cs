using System;

namespace Emporos.Core.Dto
{
    public sealed class ContactDto
    {
        public Guid ContactId { get; init; }
        public string FirstName { get; init; }
        public string Surname { get; init; }
        public DateTime DateOfBirth { get; init; }
        public string Email { get; init; }
    }
}