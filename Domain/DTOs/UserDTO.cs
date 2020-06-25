using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTOs
{
    public class UserDTO
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("user_name")]
        public string UserName { get; set; }
        [JsonProperty("last_login_date")]
        public string LastLogin { get; set; }
    }
}
