using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.DTOs;
using Domain.Helpers;
using Domain.Models;
using Domain.Service;
using Domain.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using restfulapi.ReturnObjects;

namespace restfulapi.Controllers
{
    //I'm keeping this project very simple so im authenticating the user myself
    //No security concerns this time
    [Route("api/user")]
    [EnableCors("_allowFrom")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IMapper _mapper;
        public UserController(UserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("getuser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetUser([FromQuery]Guid user_id)
        {
            var results = new GenericReturnObject<UserDTO>();
            try
            {
                if (user_id == Guid.Empty)
                {
                    results.Message = "Invalid userid";
                    results.Success = false;
                    return BadRequest(results);
                }
                var user = await _userService.GetUser(user_id);
                if(user == default)
                {
                    results.Message = "User doesn't exist";
                    results.Success = false;
                    return BadRequest(results);
                }
                results.Values = _mapper.Map<User, UserDTO>(user);
            }
            catch (Exception ex)
            {
                results.Success = false;
                results.Message = ex.Message;
            }
            return Ok(JsonConvert.SerializeObject(results));
        }
        [HttpGet("getnewuser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetNewUser()
        {
            var results = new GenericReturnObject<UserDTO>();
            var animals = new AnimalList();
            var colors = new ColorList();
            var images = new Images();
            try
            {
                var userColor = colors.ReturnRandomColor();
                var userAnimal = animals.ReturnAnimal();
                var profilePicture = images.ReturnRandomImage();

                var userName = $"{ userColor.Item1} { userAnimal }";
                var newUser = new User
                {
                    Id = Guid.NewGuid(),
                    LastTimeLogged = DateTimeOffset.Now,
                    Password = Guid.NewGuid().ToString(),
                    UserName = userName,
                    PrimaryColorHex = userColor.Item2,
                    Icon = profilePicture
                };
                var dbUser = _userService.GetUser(newUser.UserName);
                while (dbUser != default)
                {
                    var retryNewUser = new User
                    {
                        Id = Guid.NewGuid(),
                        LastTimeLogged = DateTimeOffset.Now,
                        Password = Guid.NewGuid().ToString(),
                        UserName = userName,
                        PrimaryColorHex = userColor.Item2,
                        Icon = profilePicture
                    };
                    dbUser = _userService.GetUser(retryNewUser.UserName);
                }
                results.Success = await _userService.CreateUser(newUser);
                results.Values = new UserDTO
                {
                    UserName = newUser.UserName,
                    Id = newUser.Id,
                    LastLogin = newUser.LastTimeLogged.Date.ToShortDateString(),
                    PrimaryColorHex = userColor.Item2,
                    Icon = profilePicture
                };
            }
            catch (Exception ex)
            {
                results.Success = false;
                results.Message = ex.Message;
            }
            return Ok(JsonConvert.SerializeObject(results));
        }
        [HttpGet("searchContactByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> SearchContactByName([FromQuery]Guid user_id,[FromQuery]string contact_name)
        {
            var results = new GenericReturnObject<List<UserDTO>>();
            try
            {
                if(user_id == Guid.Empty || String.IsNullOrEmpty(contact_name))
                {
                    results.Success = false;
                    return BadRequest(results);
                }
                var userMakingRequest = await _userService.GetUser(user_id);
                if (userMakingRequest == default)
                {
                    results.Success = false;
                    return BadRequest(results);
                }
                var userList = await _userService.GetUserByName(contact_name, user_id);
                //TODO FIX MAPPING HERE
                results.Values = userList.Select(x => new UserDTO
                {
                    UserName = x.UserName,
                    Id = x.Id
                }).ToList();
            }
            catch (Exception ex)
            {
                results.Success = false;
                results.Message = ex.Message;
            }
            return Ok(JsonConvert.SerializeObject(results));
        }
    }
}
