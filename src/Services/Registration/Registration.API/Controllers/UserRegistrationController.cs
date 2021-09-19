using System;
using System.Collections.Generic;
using System.Net;
using Core.DTO;
using Core.Repositories;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Registration.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserRegistrationController : ControllerBase
    {
      
        private readonly IUserRegistrationRepository _userRegistrationRepository;

        public UserRegistrationController(IUserRegistrationRepository userRegistrationRepository)
        {
            _userRegistrationRepository = userRegistrationRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<User>), (int)HttpStatusCode.OK)]
        public ActionResult<IEnumerable<User>> Get()
        {
            return Ok(_userRegistrationRepository.GetAll());
        }
        
        [HttpPost]
        public ActionResult Create([FromBody] UserDto user)
        {
            return Ok(_userRegistrationRepository.Create(user));
        }
    
        [HttpDelete]
        public  ActionResult Delete(Guid id)
        {
            return Ok(_userRegistrationRepository.Delete(id));
        }
    }
}