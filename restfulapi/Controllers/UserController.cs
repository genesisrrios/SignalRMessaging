using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using restfulapi.ReturnObjects;

namespace restfulapi.Controllers
{
    //I'm keeping this project very simple so im authenticating the user myself and doing everything very simple
    //No security concerns this time
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("getuser/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult GetUser(Guid userId)
        {
            var results = new GenericReturnObject<User>();
            try
            {
                results.Values = _userService.GetUser(userId);
                if (userId == Guid.Empty)
                {
                    results.Message = "Invalid userid";
                    results.Success = false;
                    return BadRequest(results);
                }
            }
            catch (Exception ex)
            {
                results.Success = false;
                results.Message = ex.Message;
            }
            return Ok();
        }
        [HttpGet("getuser/username={username}&password={password}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult GetUser(string username, string password)
        {
            var results = new GenericReturnObject<User>();
            try
            {
                if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
                {
                    results.Message = "Invalid username or password";
                    results.Success = false;
                    return BadRequest(results);
                }
                else
                {
                    results.Values = _userService.GetUser(username, password);
                }
            }
            catch (Exception ex)
            {
                results.Success = false;
                results.Message = ex.Message;
            }
            return Ok(results);
        }
        [HttpGet("login/username={username}&password={password}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> LoginUser(string username, string password)
        {
            var results = new GenericReturnObject<User>();
            try
            {
                if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
                {
                    results.Message = "Invalid username or password";
                    results.Success = false;
                    return BadRequest(results);
                }
                else
                {
                    var user = _userService.GetUser(username, password);
                    results.Values = user;
                    user.LastTimeLogged = DateTimeOffset.Now;
                    await _userService.UpdateUser(user);
                }
            }
            catch (Exception ex)
            {
                results.Success = false;
                results.Message = ex.Message;
            }
            return Ok(results);
        }

        [HttpPost("createuser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateUser([FromBody]User user)
        {
            var results = new GenericReturnObject<User>();
            try
            {
                if(String.IsNullOrEmpty(user.UserName )|| String.IsNullOrEmpty(user.Password))
                {
                    results.Success = false;
                    results.Message = "Invalid values provided";
                    return BadRequest(results);
                }
                var newUser = new User
                {
                    Id = Guid.NewGuid(),
                    LastTimeLogged = DateTimeOffset.Now,
                    Password = user.Password,
                    UserName = user.UserName

                };
                var dbUser = _userService.GetUser(user.UserName, user.Password);
                if(dbUser != default)
                {
                    results.Success = false;
                    results.Message = "User already exists";
                    return BadRequest(results);
                }
                results.Success = await _userService.CreateUser(newUser);
                results.Values = newUser;
            }
            catch (Exception ex)
            {
                results.Success = false;
                results.Message = ex.Message;
            }
            return Ok(results);
        }
    }
}
