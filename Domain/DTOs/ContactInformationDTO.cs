using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTOs
{
    public class ContactInformationDTO
    {
        [JsonProperty("user_name")]
        public string UserName { get; set; }
        [JsonProperty("last_message_excerpt")]
        public string LastMessage { get; set; }
        [JsonProperty("user_id")]
        public Guid UserId { get; set; }
    }
}
