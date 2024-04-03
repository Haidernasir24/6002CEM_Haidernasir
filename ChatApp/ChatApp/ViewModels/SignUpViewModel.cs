namespace ChatApp.ViewModels;

using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ChatApp.Services;

public class SignUpViewModel : INotifyPropertyChanged
{
    private string _displayName;
    private string _email;
    private string _password;
    private string _repeatPassword;
    private bool _isBusy;
    public event PropertyChangedEventHandler PropertyChanged;
    public Action<string, string> DisplayMessageAction { get; set; }
    public string DisplayName
    {
        get => _displayName;
        set => SetProperty(ref _displayName, value);
    }

    
    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }

    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public string RepeatPassword
    {
        get => _repeatPassword;
        set => SetProperty(ref _repeatPassword, value);
    }

    public ICommand SignUpCommand { get; }

    public SignUpViewModel()
    {
        SignUpCommand = new Command(async () => await SignUpAsync());
    }

    private async Task SignUpAsync()
    {
        // Check if name is empty
        if (string.IsNullOrWhiteSpace(DisplayName))
        {
            DisplayMessageAction?.Invoke("Error", "Name cannot be empty.");
            return;
        }

        // Basic email validation
        if (string.IsNullOrWhiteSpace(Email) || !Email.Contains("@"))
        {
            DisplayMessageAction?.Invoke("Error", "Please enter a valid email.");
            return;
        }

        // Check if password is at least 8 characters long
        if (string.IsNullOrWhiteSpace(Password) || Password.Length < 8)
        {
            DisplayMessageAction?.Invoke("Error", "Password must be at least 8 characters long.");
            return;
        }

        // Check if passwords match
        if (Password != RepeatPassword)
        {
            DisplayMessageAction?.Invoke("Error", "Passwords do not match.");
            return;
        }
        IsBusy = true;
        try
        {
            var authService = new FirebaseAuthService();
            var authResponse = await authService.SignUpWithEmailAndPassword(Email, Password, DisplayName);

            if (!string.IsNullOrEmpty(authResponse.userId))
            {
                IsBusy = false;
                // Sign up successful
                DisplayMessageAction?.Invoke("Success", "You have been signed up successfully.");
                // Navigate or update UI accordingly
            }
        }
        catch (Exception ex)
        {
            IsBusy = false;
            DisplayMessageAction?.Invoke("Error", $"Sign up failed: {ex.Message}");
        }
        IsBusy = false;
    }


    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action onChanged = null)
    {
        if (EqualityComparer<T>.Default.Equals(backingStore, value))
            return false;

        backingStore = value;
        onChanged?.Invoke();
        OnPropertyChanged(propertyName);
        return true;
    }
}

