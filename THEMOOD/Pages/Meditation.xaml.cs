using Microsoft.Maui.Controls;
using Microsoft.Maui;
using System.Collections.Generic;
using System;
using System.Timers;
using System.Linq;
using CommunityToolkit.Maui.Views;

namespace THEMOOD.Pages;

public partial class Meditation : ContentView
{
    private double _lastScrollY;
    private System.Timers.Timer _scrollTimer;
    private const double ItemHeight = 750;
    private int _currentVideoIndex = -1;
    private MediaElement _currentMediaElement;

    public Meditation()
    {
        InitializeComponent();
        var videoUrls = new List<string>
        {
            "https://tinyurl.com/mtcjhcs8", // Video 2
            "https://tinyurl.com/2k3bxdx9", // Video 1
            "https://tinyurl.com/5b8bpvjc"  // Video 3
        };
        VideoFeed.ItemsSource = videoUrls;

        // Initialize the timer
        _scrollTimer = new System.Timers.Timer(200);
        _scrollTimer.Elapsed += ScrollTimer_Elapsed;
        _scrollTimer.AutoReset = false;

        // Set initial video to play when loaded
        VideoFeed.Loaded += OnVideoFeedLoaded;
    }

    private void OnVideoFeedLoaded(object sender, EventArgs e)
    {
        // Start playing the first video when the feed loads
        PlayVideoAtIndex(0);
    }

    private void OnScrolled(object sender, ItemsViewScrolledEventArgs e)
    {
        _lastScrollY = e.VerticalOffset;
        _scrollTimer.Stop();
        _scrollTimer.Start();
    }

    private void ScrollTimer_Elapsed(object sender, ElapsedEventArgs e)
    {
        _scrollTimer.Stop();
        MainThread.BeginInvokeOnMainThread(() =>
        {
            int targetIndex = (int)Math.Round(_lastScrollY / ItemHeight);
            VideoFeed.ScrollTo(targetIndex, position: ScrollToPosition.Start, animate: true);

            // Only change videos if the index has changed
            if (_currentVideoIndex != targetIndex)
            {
                // Stop previous video if any
                if (_currentMediaElement != null)
                {
                    _currentMediaElement.Pause();
                    _currentMediaElement = null;
                }

                // Play the new video
                PlayVideoAtIndex(targetIndex);
            }
        });
    }

    private void PlayVideoAtIndex(int index)
    {
        if (index < 0 || index >= ((List<string>)VideoFeed.ItemsSource).Count)
            return;

        // Stop tracking previous video
        _currentVideoIndex = index;
        _currentMediaElement = null;

        // Get the container for the video at the specified index
        var container = VideoFeed.GetVisualTreeDescendants()
            .OfType<Grid>()
            .ElementAtOrDefault(index);

        if (container != null)
        {
            // Find the MediaElement within the Grid
            _currentMediaElement = container.Children.OfType<MediaElement>().FirstOrDefault();

            if (_currentMediaElement != null)
            {
                // Stop any other playing videos just to be safe
                foreach (var grid in VideoFeed.GetVisualTreeDescendants().OfType<Grid>())
                {
                    var mediaElement = grid.Children.OfType<MediaElement>().FirstOrDefault();
                    if (mediaElement != null && mediaElement != _currentMediaElement)
                    {
                        mediaElement.Pause();
                    }
                }

                // Start playing the current video
                _currentMediaElement.Play();
            }
        }
    }
}