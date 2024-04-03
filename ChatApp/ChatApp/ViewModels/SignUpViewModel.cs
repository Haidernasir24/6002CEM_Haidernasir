namespace ChatApp.ViewModels;

using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;


public class SignUpViewModel : INotifyPropertyChanged
{
    private string _firstName;
    private string _lastName;
    private string _email;
    private string _password;
    private string _repeatPassword;

    public event PropertyChangedEventHandler PropertyChanged;

    public string FirstName
    {
        get => _firstName;
        set => SetProperty(ref _firstName, value);
    }

    public string LastName
    {
        get => _lastName;
        set => SetProperty(ref _lastName, value);
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
        // Implement sign-up logic here
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

