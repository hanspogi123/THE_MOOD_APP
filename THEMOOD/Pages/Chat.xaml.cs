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
            
            // Scroll to bottom when view appears
            this.Loaded += async (s, e) => await ScrollToBottom();
        }

        private async Task ScrollToBottom()
        {
            if (_isScrolling) return;

            _isScrolling = true;
            try
            {
                // Wait for the UI to update
                await Task.Delay(100);
                
                if (_viewModel.Messages.Count > 0)
                {
                    // Scroll to the last item
                    MessagesCollection.ScrollTo(_viewModel.Messages.Count - 1, animate: false);
                }
            }
            finally
            {
                _isScrolling = false;
            }
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
                    await Task.Delay(100);
                    
                    // Scroll to the last item
                    MessagesCollection.ScrollTo(_viewModel.Messages.Count - 1, animate: false);
                }
                finally
                {
                    _isScrolling = false;
                }
            }
        }
    }
}