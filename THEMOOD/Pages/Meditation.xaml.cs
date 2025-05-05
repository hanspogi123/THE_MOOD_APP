using Microsoft.Maui.Controls;
using System;
using System.Linq;
using CommunityToolkit.Maui.Views;

namespace THEMOOD.Pages;

public partial class Meditation : ContentView
{

    public Meditation()
    {
        InitializeComponent();
    }

    private void ContentView_Unloaded(object sender, EventArgs e)
    {
        if (VideoPlayer != null)
        {
            VideoPlayer.Stop();  // Optional: resets to start
        }
    }
}
