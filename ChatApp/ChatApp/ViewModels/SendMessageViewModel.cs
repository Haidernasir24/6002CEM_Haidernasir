namespace ChatApp.ViewModels;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ChatApp.Models;
using ChatApp.Services; // Assuming this namespace contains your database service
using Newtonsoft.Json.Linq;
using System.Text.Json;

public class SendMessageViewModel : INotifyPropertyChanged
{
    private string _userId = null;
    public List<User> users;
    // Ensure ChatEntries is initialized before use.
    public ObservableCollection<ChatEntry> ChatEntries { get; set; } = new ObservableCollection<ChatEntry>();

    public SendMessageViewModel()
    {
        _userId = Preferences.Get("userId", "");
        var jsonString = Preferences.Get("usersKey", string.Empty);
        if (!string.IsNullOrEmpty(jsonString))
        {
            users = JsonSerializer.Deserialize<List<User>>(jsonString);
        }
        if (string.IsNullOrEmpty(_userId)) return;

        // This line is now safe because ChatEntries has been initialized.
        LoadChatEntries();
    }

    private async void LoadChatEntries()
    {
        
       // users = await _databaseService.GetUsersAsync(); // This method should fetch users from your local DB

        foreach (var user in users)
        {
            var userName = user.name;
            ChatEntries.Add(new ChatEntry { Name = userName});
        }
    }
  

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
