﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.DTOs;
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
    //I'm keeping this project very simple so im authenticating the user myself and doing everything very simple
    //No security concerns this time
    [Route("api/user")]
    [EnableCors("_allowFrom")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly ContactService _contactService;
        private readonly IMapper _mapper;
        public UserController(UserService userService, IMapper mapper, ContactService contactService)
        {
            _userService = userService;
            _mapper = mapper;
            _contactService = contactService;
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
                results.Values = _mapper.Map<User, UserDTO>(user);
            }
            catch (Exception ex)
            {
                results.Success = false;
                results.Message = ex.Message;
            }
            return Ok(JsonConvert.SerializeObject(results));
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
                var userColor = _userService.GenerateUserColor();
                var userAnimal = _userService.GenerateUserAnimal();
                var profilePicture = _userService.GetUserImage();

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
                var dbUser = _userService.GetUser(newUser.UserName, newUser.Password);
                while (dbUser != default)
                {
                    var retryNewUser = new User
                    {
                        Id = Guid.NewGuid(),
                        LastTimeLogged = DateTimeOffset.Now,
                        Password = Guid.NewGuid().ToString(),
                        UserName = userName,
                        PrimaryColorHex = userColor.Item2
                    };
                    dbUser = _userService.GetUser(retryNewUser.UserName, retryNewUser.Password);
                }
                results.Success = await _userService.CreateUser(newUser);
                results.Values = new UserDTO
                {
                    UserName = newUser.UserName,
                    Id = newUser.Id,
                    LastLogin = newUser.LastTimeLogged.Date.ToShortDateString(),
                    PrimaryColorHex = userColor.Item2
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
        [HttpPost("addContact")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddContact([FromBody]Contact contact)
        {
            var results = new GenericReturnObject<string>();
            try
            {
                if (contact.ContactId == Guid.Empty)
                {
                    results.Success = false;
                    results.Message = "Empty Contact ID";
                    return BadRequest(JsonConvert.SerializeObject(results));
                };
                if (contact.UserId == Guid.Empty)
                {
                    results.Success = false;
                    results.Message = "Empty UserId";
                    return BadRequest(JsonConvert.SerializeObject(results));
                };

                var userRequestingToAddContact = await _userService.GetUserById(contact.UserId);
                if (userRequestingToAddContact == default)
                {
                    results.Success = false;
                    results.Message = "User doesn't exist";
                    return BadRequest(JsonConvert.SerializeObject(results));
                }

                var contactToAdd = await _userService.GetUserById(contact.ContactId);
                if (contactToAdd == default && contactToAdd?.Id == contact.ContactId)
                {
                    results.Success = false;
                    results.Message = "Contact doesn't exist";
                    return BadRequest(JsonConvert.SerializeObject(results));
                }
                var addContactResults = await _contactService.AddContact(contact);
                if(!addContactResults.Item1 && addContactResults.Item2 > 0)
                {
                    results.Success = false;
                    results.Message = "";//Error fix later
                    return BadRequest(JsonConvert.SerializeObject(results));
                }
            }
            catch (Exception ex)
            {
                results.Success = false;
                results.Message = ex.Message;
            }
            results.Success = true;
            return Ok(JsonConvert.SerializeObject(results));
        }
    }
}
