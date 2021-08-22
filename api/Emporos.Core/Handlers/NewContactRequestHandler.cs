using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Emporos.Core.Domain;
using Emporos.Core.Dto;
using Emporos.Core.Stores;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Emporos.Core.Handlers
{
    public sealed class NewContactRequestHandler :
        IRequestHandler<NewContactRequest, OperationResult<ContactDto>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<NewContactRequestHandler> _logger;
        private readonly IMapper _mapper;

        public NewContactRequestHandler(ApplicationDbContext dbContext, IMapper mapper,
            ILogger<NewContactRequestHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<OperationResult<ContactDto>> Handle(NewContactRequest request,
            CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            _logger.LogInformation($"Create new contact for {request.ContactRequest}");

            try
            {
                var contact = new Contact
                {
                    ContactId = Guid.NewGuid(),
                    FirstName = request.ContactRequest.FirstName,
                    Surname = request.ContactRequest.Surname,
                    DateOfBirth = request.ContactRequest.DateOfBirth,
                    Email = request.ContactRequest.Email,
                };

                _dbContext.Contacts.Add(contact);
                await _dbContext.SaveChangesAsync(cancellationToken);

                _logger.LogInformation($"New contact for {request.ContactRequest} has been created successfully");

                return OperationResult<ContactDto>.Success(_mapper.Map<ContactDto>(contact));
            }
            catch (Exception e)
            {
                var message = $"Error in creating the contact for {request.ContactRequest}";

                _logger.LogError(message, e);
                return OperationResult<ContactDto>.Error(message);
            }
        }
    }
}