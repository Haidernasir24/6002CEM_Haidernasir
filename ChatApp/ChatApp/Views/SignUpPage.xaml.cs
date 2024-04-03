using ChatApp.ViewModels;

namespace ChatApp.Views;

public partial class SignUpPage : ContentPage
{
	public SignUpPage()
	{
		InitializeComponent();
        BindingContext = new SignUpViewModel();
    }
}
