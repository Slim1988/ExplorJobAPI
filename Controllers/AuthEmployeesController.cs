using System;
using System.Collections.Generic;
using System.Linq;
using ExplorJobAPI.Domain.Services.AuthUsers;
using Microsoft.AspNetCore.Mvc;

namespace ExplorJobAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthEmployeesController
    {
        private readonly IAuthUsersService _authUsersService;

        public AuthEmployeesController(
            IAuthUsersService authUsersService
        ) {
            _authUsersService = authUsersService;
        }

        // TODO
    }
}
