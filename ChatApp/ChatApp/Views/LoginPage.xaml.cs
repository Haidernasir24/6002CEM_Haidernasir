namespace ChatApp.Views;

using ChatApp.ViewModels;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
        var viewModel = new LoginViewModel();
		InitializeComponent();
        viewModel.DisplayMessageAction = async (title, message) =>
        {
            await ShowMessage(title, message);
        };
        viewModel.NavigateToSignUpAction = async () =>
        {
            await Shell.Current.GoToAsync("///SignUpPage");

        };
        viewModel.NavigateToChatsListPage = async () =>
        {
            await Shell.Current.GoToAsync("///ChatsListPage");

        };
        BindingContext = viewModel;
        
    }

    public async Task ShowMessage(string title, string message)
    {
        await DisplayAlert(title, message, "OK");
    }
}
