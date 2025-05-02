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
            "https://v9e.tiktokcdn.com/6461ff9a5e1541237eb957c0cf80a0cd/68161c45/video/tos/useast2a/tos-useast2a-ve-0068-euttp/ogrZXoIElnRtkI3QNBFmApUDQoPRhDLefNjEm3/?a=1340&bti=OUBzOTg7QGo6OjZAL3AjLTAzYCMxNDNg&ch=0&cr=13&dr=0&er=0&lr=all&net=0&cd=0%7C0%7C0%7C&cv=1&br=7904&bt=3952&cs=0&ds=6&ft=WgzBlNM6VkywU0mr1arz7Er5SP1o-jPXtMkvY7oyqF_4&mime_type=video_mp4&qs=0&rc=Z2U1OjVnO2dmNWk0ODQ5ZkBpM291N3U5cjhndDMzZjczM0AtYV4xYmIyXjUxMjYxLi5hYSNuM3JuMmRzMTNgLS1kMWNzcw%3D%3D&vvpl=1&l=2025050221331312845C82D97B8E42E891&btag=e000a0000", // Video 2
            "https://v9e.tiktokcdn.com/aa277fc60785bedab7e19041ed66be03/68161c93/video/tos/alisg/tos-alisg-pve-0037c001/oYYziAZ4BYAWbjk6mhUTERiEBGLPK8TIiThrA/?a=1340&bti=OUBzOTg7QGo6OjZAL3AjLTAzYCMxNDNg&ch=0&cr=13&dr=0&er=0&lr=all&net=0&cd=0%7C0%7C0%7C&cv=1&br=4732&bt=2366&cs=0&ds=6&ft=arF-uqI3mDUPD12ULaAT3wUhaS2RaeF~O5&mime_type=video_mp4&qs=0&rc=Mzk8Njs4NzxmNDQ7NDpmNEBpajlmOXQ5cmVnczMzODczNEAtMy0wLzRfXzMxMzYvXjUtYSNtaC9yMmQ0amZgLS1kMS1zcw%3D%3D&vvpl=1&l=20250502213352D64B9DA5CF45679B1747&btag=e000a0000", // Video 1
            "https://v16e.tiktokcdn.com/745bc8182845b8ee38ccd6d91eddd34a/68161b7c/video/tos/maliva/tos-maliva-ve-0068c799-us/o0fcggDRAHkqQeCHJdKICPwCUifdkio2AAy5Fj/?a=1340&bti=OUBzOTg7QGo6OjZAL3AjLTAzYCMxNDNg&ch=0&cr=13&dr=0&er=0&lr=all&net=0&cd=0%7C0%7C0%7C&cv=1&br=1458&bt=729&cs=0&ds=6&ft=WgzBlNM6VkywU0mr1arz7Er5SeUo-jPXtMkvY7oyqF_4&mime_type=video_mp4&qs=0&rc=N2k2NWU4aTNpaWZpaDU7OUBpM25vN2Y6ZjY3bjMzZzczNEBiM14zMzZeXi4xYDNhYS8wYSNlNXEwcjRnbGhgLS1kMS9zcw%3D%3D&vvpl=1&l=202505022130191986F2404DE2163314B3&btag=e00098000"  // Video 3
        };
        VideoFeed.ItemsSource = videoUrls;

        // Initialize the timer
        _scrollTimer = new System.Timers.Timer(500);
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