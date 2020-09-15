﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTOs
{
    public class ConversationDTO
    {
        [JsonProperty("from_id")]
        public Guid FromId { get; set; }
        [JsonProperty("to_id")]
        public Guid ToId { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("profile_picture")]
        public string ProfilePicture { get; set; }
        [JsonProperty("time_sent")]
        public string TimeSent { get; set; }
    }
}
