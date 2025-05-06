using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using THEMOOD.ViewModels;
using THEMOOD.Services;

namespace THEMOOD.ViewModels
{
    class Chat_VM : INotifyPropertyChanged
    {
        private readonly ChatService _chatService;
        private string _userMessage;
        private bool _isSending;

        public ObservableCollection<ChatMessage> Messages { get; }
        public ICommand SendMessageCommand { get; }
        public ICommand ClearConversationCommand { get; }

        public string UserMessage
        {
            get => _userMessage;
            set
            {
                if (_userMessage != value)
                {
                    _userMessage = value;
                    OnPropertyChanged();
                    ((Command)SendMessageCommand).ChangeCanExecute();
                }
            }
        }

        public bool IsSending
        {
            get => _isSending;
            set
            {
                if (_isSending != value)
                {
                    _isSending = value;
                    OnPropertyChanged();
                    ((Command)SendMessageCommand).ChangeCanExecute();
                }
            }
        }

        public Chat_VM()
        {
            _chatService = new ChatService("AIzaSyCOwo-lCI40SB7HRW6ad1xV28pxUIIoDjQ");
            Messages = new ObservableCollection<ChatMessage>();
            SendMessageCommand = new Command(async () => await SendMessageAsync(), CanSendMessage);
            ClearConversationCommand = new Command(ClearConversation);

            // Add welcome messages
            AddWelcomeMessages();
        }

        private void AddWelcomeMessages()
        {
            Messages.Add(new ChatMessage
            {
                Text = "Hello! I'm your mood assistant developed by The Mood Co.",
                IsFromUser = false
            });

            Messages.Add(new ChatMessage
            {
                Text = "How are you feeling today?",
                IsFromUser = false
            });
        }

        private void ClearConversation()
        {
            Messages.Clear();
            _chatService.ClearConversation();
            AddWelcomeMessages();
        }

        private bool CanSendMessage()
        {
            return !string.IsNullOrWhiteSpace(UserMessage) && !IsSending;
        }

        private async Task SendMessageAsync()
        {
            if (string.IsNullOrWhiteSpace(UserMessage)) return;

            try
            {
                // Add user message to chat
                var userMessageText = UserMessage.Trim();
                var userMessage = new ChatMessage 
                { 
                    Text = userMessageText, 
                    IsFromUser = true,
                    Timestamp = DateTime.Now
                };
                Messages.Add(userMessage);

                // Clear input and show loading state
                UserMessage = string.Empty;
                IsSending = true;
                OnPropertyChanged(nameof(UserMessage));

                // Get AI response
                var response = await _chatService.SendMessageAsync(userMessageText);

                // Add AI response to chat
                var aiMessage = new ChatMessage 
                { 
                    Text = response, 
                    IsFromUser = false,
                    Timestamp = DateTime.Now
                };
                Messages.Add(aiMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SendMessageAsync: {ex.Message}");
                Messages.Add(new ChatMessage 
                { 
                    Text = "Sorry, I encountered an error. Please try again.", 
                    IsFromUser = false,
                    Timestamp = DateTime.Now
                });
            }
            finally
            {
                IsSending = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}