using System;
using System.Collections.Generic;
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
    public sealed class GetContactListRequestHandler :
        IRequestHandler<GetContactListRequest, OperationResult<IEnumerable<ContactDto>>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<GetContactListRequestHandler> _logger;

        public GetContactListRequestHandler(ApplicationDbContext dbContext,
            IMapper mapper,
            ILogger<GetContactListRequestHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<OperationResult<IEnumerable<ContactDto>>> Handle(GetContactListRequest request,
            CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            _logger.LogInformation($"Load the list of the contacts");

            try
            {
                var contacts = await _dbContext.Contacts.ToArrayAsync(cancellationToken: cancellationToken);

                _logger.LogInformation($"{contacts.Length} contacts have been loaded");

                return OperationResult<IEnumerable<ContactDto>>.Success(_mapper.Map<IEnumerable<ContactDto>>(contacts));
            }
            catch (Exception e)
            {
                var message = $"Error in loading the list of contacts";

                _logger.LogError(message, e);
                return OperationResult<IEnumerable<ContactDto>>.Error(message);
            }
        }
    }
}