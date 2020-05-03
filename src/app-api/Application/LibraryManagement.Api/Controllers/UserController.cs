using System.Threading.Tasks;
using LibraryManagement.Api.Framework.Controllers;
using LibraryManagement.Application.Model.UserModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class UserController: BaseApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bus"></param>
        public UserController(IMediator bus) 
            : base(bus)
        {
        }


        /// <summary>
        ///  User Signin
        /// </summary>
        /// <returns>User data</returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest model)
        {
            return ToResult(await Bus.Send(model));
        }
    }
}
