using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;

namespace THEMOOD.Services
{
    public class PopupService
    {
        private static Action<CommunityToolkit.Maui.Views.Popup> _showPopupAction;
        private static Func<CommunityToolkit.Maui.Views.Popup, Task<object>> _showPopupAsyncFunc;

        public static void Initialize(
            Action<CommunityToolkit.Maui.Views.Popup> showPopupAction,
            Func<CommunityToolkit.Maui.Views.Popup, Task<object>> showPopupAsyncFunc)
        {
            _showPopupAction = showPopupAction;
            _showPopupAsyncFunc = showPopupAsyncFunc;
        }

        public static void ShowPopup(CommunityToolkit.Maui.Views.Popup popup)
        {
            _showPopupAction?.Invoke(popup);
        }

        public static async Task<object> ShowPopupAsync(CommunityToolkit.Maui.Views.Popup popup)
        {
            if (_showPopupAsyncFunc == null)
                throw new InvalidOperationException("PopupService is not initialized with ShowPopupAsync function");

            return await _showPopupAsyncFunc.Invoke(popup);
        }
    }
}