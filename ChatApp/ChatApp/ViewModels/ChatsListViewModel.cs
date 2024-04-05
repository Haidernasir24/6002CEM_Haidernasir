namespace ChatApp.ViewModels;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ChatApp.Models;
using ChatApp.Services; // Assuming this namespace contains your database service
using Newtonsoft.Json.Linq;

public class ChatsListViewModel : INotifyPropertyChanged
{
    private string _userId = null;
    private readonly DatabaseService _databaseService;
    private readonly FirebaseListenerService firebaseListenerService;
    public List<User> users;
    List<ChatMessagesModel> messagesInDb;
    // Ensure ChatEntries is initialized before use.
    public ObservableCollection<ChatEntry> ChatEntries { get; set; } = new ObservableCollection<ChatEntry>();

    public ChatsListViewModel()
    {
        _databaseService = new DatabaseService();
        _userId = Preferences.Get("userId", "");
        if (string.IsNullOrEmpty(_userId)) return;
        firebaseListenerService = new FirebaseListenerService(_databaseService, AddChatEntry);
        
        // This line is now safe because ChatEntries has been initialized.
        LoadChatEntries();
        Task.Run(() => firebaseListenerService.StartListeningAsync());
    }

    private async void LoadChatEntries()
    {
        await _databaseService.InitializeDatabaseAsync();
        messagesInDb = await _databaseService.GetMessagesAsync();
        users = await _databaseService.GetUsersAsync(); // This method should fetch users from your local DB

        foreach(var msg in messagesInDb)
        {
            var text = msg.text;
            var userName = users.Where(m => m.userId.Equals(msg.userId))
                         .Select(m => m.name)
                         .FirstOrDefault();
            ChatEntries.Add(new ChatEntry { Name = userName, LastMessage = text });
        }
       
    }
    private async void AddChatEntry(JObject entry)
    {
        
        if (entry.ContainsKey("messages"))
        {


          //  List<ChatMessagesModel> messagesInDb = await _databaseService.GetMessagesAsync();
            var msgsIds = messagesInDb.Select(m => m.messageId.ToString()).ToList();
            JObject messages = ((entry["messages"] as JObject)[_userId] as JObject);
            if(messages != null) 
            messages.Remove("ignore");
            var keysInRTDB = messages.Properties();
            foreach (JProperty key in keysInRTDB)
            {
                string keytouse = key.Name ;
                if (msgsIds.Contains(keytouse))
                {
                    Console.WriteLine(keytouse + " is already there");
                    continue;
                }
                Console.WriteLine("Recreived + " + messages.ToString());
                Console.WriteLine("key name = " + key.Name);
                Console.WriteLine("Key value " + ((JObject)key.Value).ToString());
                JObject currentMessage = key.Value as JObject;
                ChatMessagesModel currentChatMessage = new ChatMessagesModel
                {
                    messageId = keytouse,
                    userId = currentMessage["userId"].ToString(),
                    text = currentMessage["text"].ToString(),
                    wasSent = false,
                    timeStamp = (long)currentMessage["timestamp"]
                };
                _databaseService.InsertChatMessageAsync(currentChatMessage);
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    var userName = users.Where(m => m.userId.Equals(currentChatMessage.userId))
                         .Select(m => m.name)
                         .FirstOrDefault();
                    ChatEntry entry = new ChatEntry
                    {
                        Name = userName ?? "Anonymous",
                        LastMessage = currentMessage["text"].ToString(),
                    };
                    ChatEntries.Add(entry);
                });
            }
        }
        if(entry.ContainsKey("users"))
        {
            Console.WriteLine("it does contain user");
            var userIds = users.Select(m => m.userId.ToString()).ToList();
            Console.WriteLine("user ids saved are " + userIds.Count);
            JObject usrInRTDB = (entry["users"] as JObject);
            Console.WriteLine("user in rtdb are " + usrInRTDB.ToString());
            usrInRTDB.Remove(_userId);
            usrInRTDB.Remove("holder");
            var keysInRTDB = usrInRTDB.Properties();
            foreach(JProperty key in keysInRTDB)
            {
                String keytouse = key.Name;
                if (userIds.Contains(keytouse)) return;
                
                JObject currentUser = key.Value as JObject;
                if (currentUser == null) Console.WriteLine("current not found " + keytouse);
                Console.WriteLine("currentuser found");
                Console.WriteLine(currentUser["displayName"].ToString());

                User newUser = new User
                {
                    userId = keytouse,
                    name = currentUser["displayName"].ToString(),
                    id = users.Count
                };
                users.Add(newUser);
                _databaseService.InsertUserAsync(newUser);
                Console.WriteLine("user " + newUser.name + " added succesfully");
            }
        }
        
        // Ensure modifications are performed on the main thread
        
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
