using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace restfulapi.ReturnObjects
{
    public class GenericReturnObject<TValue>
    {
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("values")]
        public TValue Values { get; set; }
    }
}
