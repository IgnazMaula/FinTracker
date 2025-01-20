using Microsoft.AspNetCore.Mvc;
using FinTracker.Domain.Interfaces;
using MediatR;
using FinTracker.Application.Features.Users.Query;
using FinTracker.Application.Features.Users.Command;
using System;
using System.Threading.Tasks;

namespace FinTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var query = new GetAllUsersQuery();
                var result = await _mediator.Send(query);

                if (result.Status != 200)
                    return StatusCode(result.Status, new { message = result.Message });

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            try
            {
                var query = new GetUserByIdQuery(id);
                var result = await _mediator.Send(query);

                if (result.Status != 200)
                    return StatusCode(result.Status, new { message = result.Message });

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);

                if (result.Status != 200)
                    return StatusCode(result.Status, new { message = result.Message });

                return CreatedAtAction(nameof(GetUserById), new { id = result.Data.Id }, result.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserCommand command)
        {
            try
            {
                command.Id = id;
                var result = await _mediator.Send(command);

                if (result.Status != 200)
                    return StatusCode(result.Status, new { message = result.Message });

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                var command = new DeleteUserCommand(id);
                var result = await _mediator.Send(command);

                if (result.Status != 200)
                    return StatusCode(result.Status, new { message = result.Message });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }
    }
}
