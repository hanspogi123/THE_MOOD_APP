using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace THEMOOD.ViewModels
{
    public partial class ChatMessage:ObservableObject
    {
        [ObservableProperty]
        public string _text;

        [ObservableProperty]
        public bool _isFromUser;

        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
