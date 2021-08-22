using System;
using Emporos.Core.Domain;
using Emporos.Core.Dto;
using MediatR;

namespace Emporos.Core.Handlers
{
    public sealed class GetContactRequest : IRequest<OperationResult<ContactDto>>
    {
        public GetContactRequest(Guid contactId)
        {
            ContactId = contactId;
        }

        public Guid ContactId { get; }
    }
}