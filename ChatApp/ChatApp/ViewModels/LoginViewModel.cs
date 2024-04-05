namespace ChatApp.ViewModels;
using System;
using System.Windows.Input;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using ChatApp.Services; // Ensure this is pointing to where your FirebaseAuthService is located
using Newtonsoft.Json;
using ChatApp.Models;
public class LoginViewModel : BaseViewModel
{
    private FirebaseAuthService _authService;

    public string Email { get; set; }
    public string Password { get; set; }
    private bool _isBusy;
    public ICommand LoginCommand { get; }
    
    public ICommand NavigateToSignUpCommand { get; }

    public Action<string, string> DisplayMessageAction { get; set; }
    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }
    public LoginViewModel()
    {
        _authService = new FirebaseAuthService();

        LoginCommand = new Command(async () => await LoginWithEmailPasswordAsync());
        
        NavigateToSignUpCommand = new Command(async () => await NavigateToSignUpAsync());
    }
    public Action NavigateToSignUpAction { get; set; }
    public Action NavigateToChatsListPage { get; set; }

    private async Task LoginWithEmailPasswordAsync()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            MainThread.BeginInvokeOnMainThread(() => {
                DisplayMessageAction?.Invoke("Error", "Email and password are required.");
            });
            return;
        }
        IsBusy = true;

        try
        {
            var response = await _authService.SignInWithEmailAndPassword(Email, Password);
            var signInResponse = JsonConvert.DeserializeObject<FirebaseSignInResponse>(response);
            if (!string.IsNullOrEmpty(signInResponse.displayName) && !string.IsNullOrEmpty(signInResponse.localId))
            {
                IsBusy = false;
                Preferences.Set("IsLoggedIn", true);
                Preferences.Set("userId", signInResponse.localId);
                Preferences.Set("userName", signInResponse.displayName);
                NavigateToChatsListPage?.Invoke();
                // Proceed to navigate to the main part of your application
            }
            else
            {
                IsBusy = false;
                DisplayMessageAction?.Invoke("Error", "Login failed.");
            }
        }
        catch (Exception ex)
        {
            IsBusy = false;
            DisplayMessageAction?.Invoke("Error", "Login failed");
        }
    }

    private async Task NavigateToSignUpAsync()
    {
        NavigateToSignUpAction?.Invoke();
    }
}


