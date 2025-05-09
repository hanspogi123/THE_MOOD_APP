﻿using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;

namespace THEMOOD;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation |
    ConfigChanges.UiMode | ConfigChanges.ScreenLayout |
    ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

#if DEBUG
        AndroidEnvironment.UnhandledExceptionRaiser += (sender, args) =>
        {
            System.Diagnostics.Debug.WriteLine("Unhandled exception: " + args.Exception);
            args.Handled = true;
        };
#endif
    }
}
