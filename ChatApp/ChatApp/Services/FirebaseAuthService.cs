namespace ChatApp.Services;
using ChatApp.Models;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;

public class FirebaseAuthService
{
    private readonly string apiKey = "AIzaSyBBahdGBVKdqFPnpAbNJB8jHdjJc46KHeQ";

    public async Task<string> SignInWithEmailAndPassword(string email, string password)
    {
        using (var client = new HttpClient())
        {
            var uri = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={apiKey}";

            var payload = new
            {
                email = email,
                password = password,
                returnSecureToken = true
            };

            var jsonPayload = JsonConvert.SerializeObject(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(uri, content);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                
                

                // Assuming you have a FirebaseSignInResponse class to deserialize the JSON response
                return jsonResponse; // The Firebase ID token of the signed-in user
            }
            else
            {
                // Handle error
                var errorResponse = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to sign in: {errorResponse}");
            }
        }
    }

    public async Task<AuthResponse> SignUpWithEmailAndPassword(string email, string password, String displayName)
    {
        var signUpRequest = new SignUpRequest
        {
            email = email,
            password = password,
            displayName = displayName
        };

       
        var httpClient = new HttpClient();
        string url = $"https://us-central1-chatapp-95177.cloudfunctions.net/createUser";

        var jsonRequest = JsonConvert.SerializeObject(signUpRequest);
        var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(url, httpContent);

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<AuthResponse>(jsonResponse);
        }
        else
        {
            // Handle error response
            var errorResponse = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to sign up: {errorResponse}");
        }
    }
    public static async void SendMessage( string text, string recipientId)
    {
        string id = Preferences.Get("userId", "");
        var sendMessageParams = new SendMessageCloudFunction
        {
            userId = id,
            text = text,
            recipientId = recipientId
        };
        Console.WriteLine("in firebase => " + sendMessageParams.ToString());

        var httpClient = new HttpClient();
        string url = $"https://us-central1-chatapp-95177.cloudfunctions.net/sendMessage";

        var jsonRequest = JsonConvert.SerializeObject(sendMessageParams);
        var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(url, httpContent);
    }
}





