using System;
using Emporos.Core.Domain;
using Emporos.Core.Dto;
using MediatR;

namespace Emporos.Core.Handlers
{
    public sealed class NewContactRequest : IRequest<OperationResult<ContactDto>>
    {
        public NewContactRequest(ContactRequestDto contactRequest)
        {
            ContactRequest = contactRequest ?? throw new ArgumentNullException(nameof(contactRequest));
        }

        public ContactRequestDto ContactRequest { get; }

    }
}