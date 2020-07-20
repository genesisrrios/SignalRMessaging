using Newtonsoft.Json;
using System;

public class User
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    [JsonProperty("password")]
    public string Password { get; set; }
    public DateTimeOffset LastTimeLogged { get; set; }
    public string PrimaryColorHex { get; set; }
}
