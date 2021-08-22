using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Emporos.Core.Domain;
using Emporos.Core.Dto;
using Emporos.Core.Stores;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Emporos.Core.Handlers
{
    public sealed class GetContactRequestHandler :
        IRequestHandler<GetContactRequest, OperationResult<ContactDto>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<GetContactRequestHandler> _logger;

        public GetContactRequestHandler(ApplicationDbContext dbContext,
            IMapper mapper,
            ILogger<GetContactRequestHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<OperationResult<ContactDto>> Handle(GetContactRequest request,
            CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            _logger.LogInformation($"Load contact with id {request.ContactId}");

            try
            {
                var contact = await _dbContext.Contacts.FirstOrDefaultAsync(m => m.ContactId == request.ContactId, cancellationToken: cancellationToken);
                if (contact == null)
                {
                    var message = $"Contact with id {request.ContactId} not found";
                    _logger.LogWarning(message);
                    
                    return OperationResult<ContactDto>.Error(OperationErrorReason.ResourceNotFound, message);
                }

                _logger.LogInformation($"Contact {contact.ContactId} has been loaded");

                return OperationResult<ContactDto>.Success(_mapper.Map<ContactDto>(contact));
            }
            catch (Exception e)
            {
                var message = $"Error in loading the list of contacts";

                _logger.LogError(message, e);
                return OperationResult<ContactDto>.Error(message);
            }
        }
    }
}