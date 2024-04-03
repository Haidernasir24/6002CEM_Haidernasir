using Newtonsoft.Json;

namespace ChatApp.Models;


public class AuthResponse
{
    [JsonProperty("userId")]
    public string userId { get; set; }
}
