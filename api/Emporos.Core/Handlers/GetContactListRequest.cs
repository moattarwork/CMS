using System.Collections.Generic;
using Emporos.Core.Domain;
using Emporos.Core.Dto;
using MediatR;

namespace Emporos.Core.Handlers
{
    public sealed class GetContactListRequest : IRequest<OperationResult<IEnumerable<ContactDto>>>
    {
    }
}