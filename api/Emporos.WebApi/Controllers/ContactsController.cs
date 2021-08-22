using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Emporos.Core.Domain;
using Emporos.Core.Dto;
using Emporos.Core.Handlers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Emporos.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContactsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<ContactDto>>> Get()
        {
            var response = await _mediator.Send(new GetContactListRequest());
            if (!response.Failed)
                return Ok(response.Result);
            return response.ErrorReason == OperationErrorReason.ResourceNotFound
                ? NotFound()
                : StatusCode(StatusCodes.Status500InternalServerError);
        }        
        
        [HttpGet("{contactId:guid}")]
        public async Task<ActionResult<ContactDto>> Get(Guid contactId)
        {
            var response = await _mediator.Send(new GetContactRequest(contactId));
            if (!response.Failed)
                return Ok(response.Result);
            
            return response.ErrorReason == OperationErrorReason.ResourceNotFound
                ? NotFound()
                : StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost("")]
        public async Task<ActionResult<ContactDto>> Post([FromBody] ContactRequestDto contactRequest)
        {
            var response = await _mediator.Send(new NewContactRequest(contactRequest));
            if (response.Failed)
                return BadRequest();

            return Ok(response.Result);
        }

        [HttpPut("{contactId:guid}")]
        public async Task<ActionResult<ContactDto>> Put(Guid contactId,
            [FromBody] ContactRequestDto contactRequest)
        {
            var response = await _mediator.Send(new UpdateContactRequest(contactId, contactRequest));

            if (!response.Failed)
                return Ok(response.Result);

            return response.ErrorReason == OperationErrorReason.ResourceNotFound
                ? NotFound()
                : StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}