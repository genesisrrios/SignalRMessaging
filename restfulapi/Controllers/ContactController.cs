using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.DTOs;
using Domain.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using restfulapi.ReturnObjects;

namespace Restfulapi.Controllers
{
    [Route("api/contact")]
    [EnableCors("_allowFrom")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly ContactService _contactService;
        private readonly IMapper _mapper;
        public ContactController(IMapper mapper, ContactService contactService)
        {
            _mapper = mapper;
            _contactService = contactService;
        }
        [HttpGet("getContacts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetContacts([FromQuery] Guid user_id)
        {
            var results = new GenericReturnObject<List<ContactInformationDTO>>();
            try
            {
                if (user_id == Guid.Empty)
                {
                    results.Message = "Invalid userid";
                    results.Success = false;
                    return BadRequest(results);
                }
                results.Values = await _contactService.GetUserContacts(user_id);
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
