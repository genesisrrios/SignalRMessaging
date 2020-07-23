using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class Contact
    {
        [JsonProperty("user_id")]
        public Guid UserId { get; set; }
        [JsonProperty("contact_id")]
        public Guid ContactId { get; set; }
        [JsonProperty("is_blocked")]
        public bool IsBlocked { get; set; }
    }
}
