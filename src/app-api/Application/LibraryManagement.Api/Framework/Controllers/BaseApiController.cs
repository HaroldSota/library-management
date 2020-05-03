using System;
using LibraryManagement.Application;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Framework.Controllers
{
    /// <summary>
    ///     BaseApiController
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BaseApiController : ControllerBase
    {

        protected readonly IMediator Bus;

        /// <summary>
        ///     BaseApiController ctor.
        /// </summary>
        /// <param name="bus">Cmd. transportation buss</param>
        public BaseApiController(IMediator bus)
        {
            Bus = bus;
        }

        public Guid UserId => Guid.Parse(Request.HttpContext.User.Identity.Name);

        /// <summary>
        ///     Formats the response status to be sent to th client
        /// </summary>
        /// <typeparam name="TQueryResult">The result object type</typeparam>
        /// <param name="response">the wrapper of the result object</param>
        /// <returns></returns>
        protected IActionResult ToResult<TQueryResult>(MessageResponse<TQueryResult> response)
            => response.MessageType switch
            {
                MessageType.OK => Ok(response.Result),
                MessageType.NotFound => NotFound(response.Error),
                MessageType.Validation => BadRequest(response.Error),
                _ => Problem(response.Error.Message),
            };
    }
}
