using Microsoft.Maui.Controls;

namespace THEMOOD.Services
{
    public class PageTransitionService
    {
        public static async Task AnimatePageTransition(View oldView, View newView, ContentPresenter contentPresenter)
        {
            if (oldView != null)
            {
                // Simple fade out
                await oldView.FadeTo(0, 150, Easing.Linear);
                oldView.IsVisible = false;
            }

            // Set up new view
            newView.Opacity = 0;
            newView.IsVisible = true;

            // Simple fade in
            await newView.FadeTo(1, 150, Easing.Linear);
        }

        public static async Task AnimatePageExit(View view)
        {
            if (view != null)
            {
                await view.FadeTo(0, 150, Easing.Linear);
                view.IsVisible = false;
            }
        }
    }
} 