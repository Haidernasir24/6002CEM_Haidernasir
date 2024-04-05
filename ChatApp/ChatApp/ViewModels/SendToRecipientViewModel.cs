namespace ChatApp.ViewModels;
using ChatApp.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;

    public class SendToRecipientViewModel : INotifyPropertyChanged
    {
    private string _messageContent;
    private string recipientId;
        public string MessageContent
        {
            get => _messageContent;
            set
            {
                _messageContent = value;
                OnPropertyChanged();
            }
        }
    public Action NavigateToRecipientsList { get; set; }
    public ICommand SendMessageCommand { get; }

        public SendToRecipientViewModel()
        {
        
                recipientId = Preferences.Get("recid", "");

            SendMessageCommand = new Command(() => SendMessage());
        }

        private void SendMessage()
        {
        
        FirebaseAuthService.SendMessage(_messageContent, recipientId);
        NavigateToRecipientsList?.Invoke();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

