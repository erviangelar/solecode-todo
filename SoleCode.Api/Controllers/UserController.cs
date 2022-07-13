using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoleCode.Api.Dto;
using SoleCode.Api.Handlers.User.Queries;

namespace SoleCode.Api.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        /// <summary>
        /// User Constructor, initiation MediatR.
        /// </summary>
        public UserController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        /// <summary>
        /// Get user by username
        /// </summary>
        /// <returns>UserDTO</returns>
        [HttpGet("users")]
        [ProducesResponseType(typeof(List<UserDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> GetUser()
        {
            return Ok(await _mediator.Send(new GetUserQuery()));
        }

        /// <summary>
        /// Get user by username
        /// </summary>
        /// <returns>UserDTO</returns>
        [HttpPost("user")]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> GetUser([FromBody] UserFlter data)
        {
            return Ok(await _mediator.Send(new GetUserByUsernameQuery(data.Username)));
        }

        /// <summary>
        /// Get user by UID
        /// </summary>
        /// <returns>UserDTO</returns>
        [HttpGet("user/{uid}")]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> GetUser(Guid uid)
        {
            return Ok(await _mediator.Send(new GetUserByUIDQuery(uid)));
        }
    }
}
