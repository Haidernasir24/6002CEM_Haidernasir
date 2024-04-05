namespace ChatApp.Views;
using ChatApp.Models;
using System.Collections.ObjectModel;
using ChatApp.ViewModels;
using System.Text.Json;

public partial class ChatsListPage : ContentPage
{
   

  public ChatsListPage()
	{
		var viewModel = new ChatsListViewModel();
		InitializeComponent();
        viewModel.ChatEntries = new ObservableCollection<ChatEntry>();
		BindingContext = viewModel;
	}
    private async void SendMessage(object sender, EventArgs e)
    {
        if (BindingContext is ChatsListViewModel viewModel)
        {
            var users = viewModel.users; 
            var jsonString = JsonSerializer.Serialize(users);
            Preferences.Set("usersKey", jsonString);
            await Shell.Current.GoToAsync("//SendMessagePage");
        }
    }

    private async void Logout(object sender, EventArgs e)
    {
        Preferences.Clear();
        await Shell.Current.GoToAsync("//LoginPage");
    }
}
