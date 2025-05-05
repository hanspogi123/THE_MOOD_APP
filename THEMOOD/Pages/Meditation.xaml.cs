// Update Meditation.xaml.cs
using Microsoft.Maui.Controls;
using System;
using CommunityToolkit.Maui.Views;

namespace THEMOOD.Pages;

public partial class Meditation : ContentView
{
    private bool _isVideoLoaded = false;

    public Meditation()
    {
        InitializeComponent();

        // Use lazy loading for video
        this.Loaded += ContentView_Loaded;
    }

    private void ContentView_Loaded(object sender, EventArgs e)
    {
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            // Delay loading video to improve app startup
            Dispatcher.Dispatch(async () =>
            {
                await Task.Delay(500); // Give UI time to render
                _isVideoLoaded = true;
                LoadingIndicator.IsVisible = false;
                LoadingIndicator.IsRunning = false;
            });
        
    }

    private void ContentView_Unloaded(object sender, EventArgs e)
    {
        if (VideoPlayer != null && _isVideoLoaded)
        {
            VideoPlayer.Stop();
        }
    }
}