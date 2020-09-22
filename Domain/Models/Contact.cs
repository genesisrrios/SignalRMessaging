using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Models
{
    public class Contact
    {
        [Key]
        public int Id{ get; set; }
        [JsonProperty("user_id")]
        public Guid UserId { get; set; }
        [JsonProperty("contact_id")]
        public Guid ContactId { get; set; }
        [JsonProperty("is_blocked")]
        public bool IsBlocked { get; set; }
    }
}
