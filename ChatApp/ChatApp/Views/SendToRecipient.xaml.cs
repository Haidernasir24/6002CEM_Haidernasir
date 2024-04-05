namespace ChatApp.Views;
using ChatApp.ViewModels;
public partial class SendToRecipient : ContentPage
{
	public SendToRecipient()
	{
		var viewModel = new SendToRecipientViewModel();
		InitializeComponent();
        BindingContext = viewModel;
        viewModel.NavigateToRecipientsList = async () =>
        {
            await Shell.Current.GoToAsync("///SendMessagePage");

        };

    }
    protected override bool OnBackButtonPressed()
    {
        Shell.Current.GoToAsync("//SendMessagePage");
        return true;
    }
}
