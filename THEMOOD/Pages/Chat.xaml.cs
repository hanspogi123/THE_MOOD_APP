using Microsoft.Maui.Controls;
using THEMOOD.ViewModels;
using System.Collections.Specialized;

namespace THEMOOD.Pages
{
    public partial class Chat : ContentView
    {
        private Chat_VM _viewModel;
        private bool _isScrolling;

        public Chat()
        {
            InitializeComponent();
            _viewModel = (Chat_VM)BindingContext;
            _viewModel.Messages.CollectionChanged += Messages_CollectionChanged;
        }

        private async void Messages_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_isScrolling) return;

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                _isScrolling = true;
                try
                {
                    // Wait for the UI to update
                    await Task.Delay(50);
                    // Scroll to the bottom
                    await ChatScrollView.ScrollToAsync(0, ChatScrollView.ContentSize.Height, false);
                }
                finally
                {
                    _isScrolling = false;
                }
            }
        }
    }
}