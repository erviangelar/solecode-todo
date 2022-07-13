using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoleCode.Api.Common;
using SoleCode.Api.Dto;
using SoleCode.Api.Handlers.TodoList.Queries;
using System.Net;

namespace SoleCode.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class TodoController : BaseController
    {
        private readonly IMediator _mediator;
        /// <summary>
        /// Todo Constructor, initiation MediatR.
        /// </summary>
        public TodoController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        /// <summary>
        /// Get todo list
        /// </summary>
        /// <returns>List TodoListDto</returns>
        [HttpGet("todo-list")]
        [ProducesResponseType(typeof(TodoListDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> GetTodoList([FromQuery] PaginationModel paging, [FromQuery] FilterModel filter)
        {
            return Ok(await _mediator.Send(new GetTodoListQuery(paging, filter, GetUserClaim())));
        }

        /// <summary>
        /// Get todo list by UID
        /// </summary>
        /// <returns>TodoListDto</returns>
        [HttpGet("todo-list/{uid}")]
        [ProducesResponseType(typeof(TodoListDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> GetTodoList(Guid uid)
        {
            return Ok(await _mediator.Send(new GetTodoListByUIDQuery(uid)));
        }

        /// <summary>
        /// Create todo list
        /// </summary>
        /// <returns>Todo UID</returns>
        [HttpPost("todo-list")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Create([FromBody] TodoListRequest request)
        {
            return Ok(await _mediator.Send(new CreateCommand(request, GetUserClaim())));
        }

        /// <summary>
        /// Update todo list
        /// </summary>
        /// <returns>status</returns>
        [HttpPut("todo-list/{uid}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Update(Guid uid, [FromBody] TodoListUpdateRequest request)
        {
            return Ok(await _mediator.Send(new UpdateCommand(uid, request, GetUserClaim())));
        }

        /// <summary>
        /// Update Staus todo list
        /// </summary>
        /// <returns>status</returns>
        [HttpPut("todo-list-status/{uid}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateStatus(Guid uid, [FromBody] TodoListstatusRequest request)
        {
            return Ok(await _mediator.Send(new UpdateStatusCommand(uid, request.Status, GetUserClaim())));
        }

        /// <summary>
        /// Delete todo list
        /// </summary>
        /// <returns>status</returns>
        [HttpDelete("todo-list/{uid}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(Guid uid)
        {
            return Ok(await _mediator.Send(new DeleteCommand(uid, GetUserClaim())));
        }
    }
}
