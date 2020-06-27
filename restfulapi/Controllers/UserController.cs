﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.DTOs;
using Domain.Service;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using restfulapi.ReturnObjects;

namespace restfulapi.Controllers
{
    //I'm keeping this project very simple so im authenticating the user myself and doing everything very simple
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

        [HttpGet("getnewuser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetNewUser()
        {
            var results = new GenericReturnObject<UserDTO>();
            try
            {
                var newUser = new User
                {
                    Id = Guid.NewGuid(),
                    LastTimeLogged = DateTimeOffset.Now,
                    Password = Guid.NewGuid().ToString(),
                    UserName = _userService.generateUserName()
                };
                var dbUser = _userService.GetUser(newUser.UserName, newUser.Password);
                while (dbUser != default)
                {
                    var retryNewUser = new User
                    {
                        Id = Guid.NewGuid(),
                        LastTimeLogged = DateTimeOffset.Now,
                        Password = Guid.NewGuid().ToString(),
                        UserName = _userService.generateUserName()
                    };
                    dbUser = _userService.GetUser(retryNewUser.UserName, retryNewUser.Password);
                }
                results.Success = await _userService.CreateUser(newUser);
                results.Values = new UserDTO
                {
                    UserName = newUser.UserName,
                    Id = newUser.Id,
                    LastLogin = newUser.LastTimeLogged.Date.ToShortDateString()
                };
            }
            catch (Exception ex)
            {
                results.Success = false;
                results.Message = ex.Message;
            }
            return Ok(JsonConvert.SerializeObject(results));
        }
        [HttpGet("searchContactByName/{user_id}/{contact_name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> SearchForNewContactByName(Guid userId,string contactName)
        {
            var results = new GenericReturnObject<List<UserDTO>>();
            try
            {
                if(userId == Guid.Empty || String.IsNullOrEmpty(contactName))
                    results.Success = false;

                var userList = await _userService.GetUserByName(userId, contactName);
                results.Values = _mapper.Map<List<User>, List<UserDTO>>(userList);
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
