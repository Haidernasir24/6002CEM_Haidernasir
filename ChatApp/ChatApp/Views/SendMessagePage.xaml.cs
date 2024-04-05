namespace ChatApp.Views;
using ChatApp.ViewModels;
using ChatApp.Models;
public partial class SendMessagePage : ContentPage
{
    public SendMessagePage()
    {
        var viewModel = new SendMessageViewModel();
        InitializeComponent();
        BindingContext = viewModel;
    }
    protected override bool OnBackButtonPressed()
    {
        Shell.Current.GoToAsync("//ChatsListPage");
        return true;
    }
    private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is ChatEntry selectedEntry)
        {
            // Assuming you have a way to link ChatEntry back to User
            // For example, if ChatEntry contains UserId or similar identifier
            var viewModel = BindingContext as SendMessageViewModel;
            var user = viewModel?.users.FirstOrDefault(u => u.name == selectedEntry.Name);
            if (user != null)
            {

                Preferences.Set("recid", user.userId);
                await Shell.Current.GoToAsync("///TypeMessage");
            }

            // Deselect the item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
