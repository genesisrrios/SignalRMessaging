﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DTOs;
using Domain.HubInterfaces;
using Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using restfulapi.ReturnObjects;
using Restfulapi.Hubs;

namespace restfulapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly MessageService _messageService;
        private readonly IHubContext<MessageHub, IMessageClient> _messageClient;
        public MessageController(MessageService messageService, IHubContext<MessageHub, IMessageClient> messageHub)
        {
            _messageService = messageService;
            _messageClient = messageHub;
        }
        [HttpPost("writemessage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> WriteMessage([FromBody]Message message)
        {
            var results = new GenericReturnObject<bool>();
            try
            {
                if (String.IsNullOrEmpty(message.Content))
                {
                    results.Message = "No content";
                    results.Success = false;
                    return BadRequest();
                }
                else
                {
                    message.Id = Guid.NewGuid();
                    message.Read = false;
                    message.TimeSent = DateTimeOffset.Now;
                    results.Success = await _messageService.SendMessage(message);
                    results.Values = results.Success;
                    if (results.Success)
                        await _messageClient.Clients.All.ReceiveMessage(new MessageReceivedDTO
                        {
                            From = message.From,
                            To = message.To
                        });
                }
            }
            catch (Exception ex)
            {
                results.Success = false;
                results.Message = ex.Message;
            }
            return Ok(results);
        }
        [HttpGet("getmessages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetUserMessages([FromQuery] Guid user_id, [FromQuery]Guid contact_id)
        {
            var results = new GenericReturnObject<List<ConversationDTO>>();
            try
            {
                if (user_id == Guid.Empty)
                {
                    results.Message = "Invalid userid";
                    results.Success = false;
                    return BadRequest(results);
                }
                if(contact_id == Guid.Empty)
                {
                    results.Message = "Invalid contactid";
                    results.Success = false;
                    return BadRequest(results);
                }
                results.Success = true;
                results.Values = await _messageService.GetMessageList(user_id, contact_id);
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
