using System;
using Microsoft.AspNetCore.Mvc;

namespace VirtualSports.BE.Controllers
{
    [ApiController]
    public class AuthController : Controller
    {
        public AuthController()
        {

        }

        [HttpPost()]
        public IActionResult Register()
        {
            throw new NotImplementedException();
        }

        [HttpPost()]
        public IActionResult Login()
        {
            throw new NotImplementedException();
        }


        [HttpPut()]
        public IActionResult Logout()
        {
            throw new NotImplementedException();
        }
    }
}