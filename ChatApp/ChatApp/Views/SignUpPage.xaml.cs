using ChatApp.ViewModels;

namespace ChatApp.Views;

public partial class SignUpPage : ContentPage
{
	public SignUpPage()
	{
        var viewModel = new SignUpViewModel();
		InitializeComponent();
        viewModel.DisplayMessageAction = async (title, message) =>
        {
            await ShowMessage(title, message);
        };
        viewModel.NavigateToChatsListPage = async () =>
        {
            await Shell.Current.GoToAsync("//ChatsListPage");

        };
        BindingContext = viewModel;
    }
    public async Task ShowMessage(string title, string message)
    {
        await DisplayAlert(title, message, "OK");
    }
    protected override bool OnBackButtonPressed()
    {
        Shell.Current.GoToAsync("//LoginPage");

        return true;
    }
}
