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
    public sealed class UpdateContactRequestHandler :
        IRequestHandler<UpdateContactRequest, OperationResult<ContactDto>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<UpdateContactRequestHandler> _logger;
        private readonly IMapper _mapper;

        public UpdateContactRequestHandler(ApplicationDbContext dbContext, IMapper mapper,
            ILogger<UpdateContactRequestHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<OperationResult<ContactDto>> Handle(UpdateContactRequest request,
            CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            _logger.LogInformation($"Update contact id {request.ContactId}");

            try
            {
                var source = await _dbContext.Contacts
                    .FirstOrDefaultAsync(x => x.ContactId == request.ContactId, cancellationToken);
                
                if (source == null)
                    return OperationResult<ContactDto>.Error(OperationErrorReason.ResourceNotFound,
                        $"The contact not found for Id {request.ContactId}");

                source.FirstName = request.ContactRequest.FirstName;
                source.Surname = request.ContactRequest.Surname;
                source.DateOfBirth = request.ContactRequest.DateOfBirth;
                source.Email = request.ContactRequest.Email;
                _dbContext.Update(source);

                await _dbContext.SaveChangesAsync(cancellationToken);

                _logger.LogInformation($"Contact for Id {request.ContactId} has been updated successfully");

                return OperationResult<ContactDto>.Success(_mapper.Map<ContactDto>(source));
            }
            catch (Exception e)
            {
                var message = $"Error in updating the contact for Id {request.ContactId}";

                _logger.LogError(message, e);
                return OperationResult<ContactDto>.Error(message);
            }
        }
    }
}