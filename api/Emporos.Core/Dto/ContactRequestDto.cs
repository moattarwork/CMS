using System;

namespace Emporos.Core.Dto
{
    public sealed class ContactRequestDto
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }

        public override string ToString() => $"{FirstName} {Surname}";
    }
}