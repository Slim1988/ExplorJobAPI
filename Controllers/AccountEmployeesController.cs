using System;
using System.Collections.Generic;
using System.Linq;
using ExplorJobAPI.Domain.Repositories.Users;
using ExplorJobAPI.Domain.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace ExplorJobAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AccountEmployeesController
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IUserPhotoService _userPhotoService;

        public AccountEmployeesController(
            IUsersRepository usersRepository,
            IUserPhotoService userPhotoService
        ) {
            _usersRepository = usersRepository;
            _userPhotoService = userPhotoService;
        }

        // TODO
    }
}
