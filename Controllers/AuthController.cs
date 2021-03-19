using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace VirtualSports.BE.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class AuthController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        public AuthController()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost("register")]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult Register()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost("login")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public IActionResult Login()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPut()]
        public IActionResult Logout()
        {
            throw new NotImplementedException();
        }
    }
}