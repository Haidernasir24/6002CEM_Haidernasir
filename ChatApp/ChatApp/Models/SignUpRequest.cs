using Newtonsoft.Json;

namespace ChatApp.Models;

public class SignUpRequest
{
    [JsonProperty("email")]
    public string email { get; set; }

    [JsonProperty("password")]
    public string password { get; set; }
    [JsonProperty("displayName")]
    public string displayName { get; set; }

}
