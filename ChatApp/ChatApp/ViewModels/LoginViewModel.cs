namespace ChatApp.ViewModels;
using System;
using System.Windows.Input;
using System.Threading.Tasks;
using ChatApp.Services; // Ensure this is pointing to where your FirebaseAuthService is located

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
            var token = await _authService.SignInWithEmailAndPassword(Email, Password);
            if (!string.IsNullOrEmpty(token))
            {
                IsBusy = false;
                DisplayMessageAction?.Invoke("Success", token);
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


