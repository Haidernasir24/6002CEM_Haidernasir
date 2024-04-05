using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using ChatApp.Models;
namespace ChatApp.Services;

public class FirebaseListenerService
{
    private readonly DatabaseService _databaseService;
    private readonly string _firebaseUrl = "https://chatapp-95177-default-rtdb.firebaseio.com/.json?auth=gDyfMuTq9AgqYNLoF4uTwnBiZ2sXGqacYg2cByIu";
    private Action<JObject> _onNewMessage;
    public FirebaseListenerService(DatabaseService databaseService, Action<JObject> onNewMessage)
    {
        _databaseService = databaseService;
        _onNewMessage = onNewMessage;
    }

    public async Task StartListeningAsync()
    {
       
        await ListenForChanges(_firebaseUrl, async () => await StartListeningAsync());
    }

 
    
    public async Task ListenForChanges(string url, Func<Task> reconnectCallback)
    {
        HttpClient client = new HttpClient();
        Console.WriteLine("listen for changes started");
        try
        {
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/event-stream"));

            HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            using (var stream = await response.Content.ReadAsStreamAsync())
            using (var reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    Console.WriteLine("message from listener: " + line);
                    if (line.StartsWith("data:"))
                    {
                        line = line.Substring(6);
                        if (line.Trim(' ').ToLower() != "null")
                        {

                            JObject keyValuePairs = JObject.Parse(line);
                            String path = keyValuePairs["path"].ToString();
                            JObject data = keyValuePairs["data"] as JObject;
                            if (!path.Equals("/"))
                            {
                                Console.WriteLine("path is this " + path);
                                String[] pathsArray = path.Split('/');
                                Array.Reverse(pathsArray);
                                foreach (String pathString in pathsArray)
                                {
                                    Console.WriteLine("path strin g " + pathString);
                                    if (String.IsNullOrEmpty(pathString)) continue;
                                    Console.WriteLine("afterwards of return");
                                    JObject temp = data;
                                    data = new JObject();
                                    data[pathString] = temp;
                                }
                            }


                            Console.WriteLine("this is data that we have : " + data);
                            _onNewMessage?.Invoke(data);
                            Console.WriteLine("here we are after calling the message");
                       
                        }
                    }
                    
                    Console.WriteLine(line);
                }
            }
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
        }
        catch (WebException e)
        {
            Console.WriteLine($"WebException Caught: {e.Message}");
            // Implement your reconnection logic here
        }
        catch (Exception e)
        {
            Console.WriteLine($"General Exception Caught: {e.Message}");
            
            // Handle other exceptions that might occur
            await reconnectCallback();
        }
    }

}

