namespace ChatApp.Views;

using AndroidX.Lifecycle;
using ChatApp.ViewModels;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
        var viewModel = new LoginViewModel();
		InitializeComponent();
        viewModel.NavigateToSignUpAction = async () =>
        {
            await Shell.Current.GoToAsync("///SignUpPage");

        };
        BindingContext = viewModel;
    }
}
