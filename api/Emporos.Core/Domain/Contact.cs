using System;

namespace Emporos.Core.Domain
{
    public sealed class Contact
    {
        public Guid ContactId { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
    }
}