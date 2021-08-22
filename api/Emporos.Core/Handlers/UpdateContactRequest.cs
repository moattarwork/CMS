using System;
using Emporos.Core.Domain;
using Emporos.Core.Dto;
using MediatR;

namespace Emporos.Core.Handlers
{
    public sealed class UpdateContactRequest : IRequest<OperationResult<ContactDto>>
    {
        public UpdateContactRequest(Guid contactId, ContactRequestDto contactRequestDto)
        {
            ContactId = contactId;
            ContactRequest = contactRequestDto ?? throw new ArgumentNullException(nameof(contactRequestDto));
        }

        public Guid ContactId { get; }
        public ContactRequestDto ContactRequest { get; }
    }
}