﻿namespace ChatApp.ViewModels;
using System.Windows.Input;
using System.Threading.Tasks;


public class LoginViewModel : BaseViewModel
{
    public ICommand LoginCommand { get; }
    public ICommand LoginWithGoogleCommand { get; }
    public ICommand LoginWithFacebookCommand { get; }
    public ICommand NavigateToSignUpCommand { get; }

    // Action to display messages. Parameters are title and message.
    public Action<string, string> DisplayMessageAction { get; set; }

    public LoginViewModel()
    {
        LoginCommand = new Command(async () => await LoginWithEmailPasswordAsync());
        LoginWithGoogleCommand = new Command(async () => await LoginWithGoogleAsync());
        LoginWithFacebookCommand = new Command(async () => await LoginWithFacebookAsync());
        NavigateToSignUpCommand = new Command(async () => await NavigateToSignUpAsync());
    }

    private async Task LoginWithEmailPasswordAsync()
    {
        DisplayMessageAction?.Invoke("Login", "Logged in with Email and Password!");
    }

    private async Task LoginWithGoogleAsync()
    {
        DisplayMessageAction?.Invoke("Login", "Logged in with Google!");
    }

    private async Task LoginWithFacebookAsync()
    {
        DisplayMessageAction?.Invoke("Login", "Logged in with Facebook!");
    }

    private async Task NavigateToSignUpAsync()
    {
        DisplayMessageAction?.Invoke("Navigate", "Navigating to Sign Up...");
    }
}
