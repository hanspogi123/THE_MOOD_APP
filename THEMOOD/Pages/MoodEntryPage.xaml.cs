using THEMOOD.Controls;
using THEMOOD.PopUps;
using CommunityToolkit.Maui.Views;
using THEMOOD.Services;
using System.Threading.Tasks;
using THEMOOD.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microcharts.Maui;
using Microcharts;
using SkiaSharp; // Add this import for SKColor
using System.Collections.Specialized;

namespace THEMOOD.Pages;

public partial class MoodEntryPage : ContentView
{
    private ChartEntry[] entries;
    // Reference the singleton service
    private MoodEntryService _moodEntryService = MoodEntryService.Instance;

    // Add the DeleteCommand property declaration
    public ICommand DeleteCommand { get; private set; }

    // Define mood colors
    private readonly Dictionary<string, SKColor> _moodColors = new Dictionary<string, SKColor>
    {
        { "Happy", new SKColor(255, 193, 7) },    // Amber
        { "Sad", new SKColor(33, 150, 243) },     // Blue
        { "Neutral", new SKColor(158, 158, 158) },// Gray
        { "Angry", new SKColor(244, 67, 54) },    // Red
        { "Anxious", new SKColor(156, 39, 176) }, // Purple
        { "Content", new SKColor(76, 175, 80) },  // Green
        { "Stressed", new SKColor(255, 87, 34) }, // Deep Orange
        // Add more moods as needed
    };

    public MoodEntryPage()
    {
        InitializeComponent();

        // Initialize the DeleteCommand
        DeleteCommand = new Command<MoodEntry_VM>(DeleteMoodEntry);

        // Bind the CollectionView to the service's ObservableCollection
        MoodLog.ItemsSource = _moodEntryService.MoodEntries;

        // Subscribe to collection changes to update chart
        if (_moodEntryService.MoodEntries is ObservableCollection<MoodEntry_VM> observableMoodEntries)
        {
            observableMoodEntries.CollectionChanged += MoodEntries_CollectionChanged;
        }

        // Initial chart update
        UpdateMoodChart();
    }

    private void MoodEntries_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        // Update the chart when the collection changes
        UpdateMoodChart();
    }

    private void UpdateMoodChart()
    {
        // Count occurrences of each mood
        Dictionary<string, int> moodCounts = new Dictionary<string, int>();

        foreach (var entry in _moodEntryService.MoodEntries)
        {
            if (!string.IsNullOrEmpty(entry.Mood))
            {
                string mood = entry.Mood;
                if (moodCounts.ContainsKey(mood))
                {
                    moodCounts[mood]++;
                }
                else
                {
                    moodCounts[mood] = 1;
                }
            }
        }

        // Create chart entries from the mood counts
        List<ChartEntry> chartEntries = new List<ChartEntry>();

        foreach (var pair in moodCounts)
        {
            // Get color for this mood, or default to a random color
            SKColor color = _moodColors.ContainsKey(pair.Key)
                ? _moodColors[pair.Key]
                : new SKColor((byte)Random.Shared.Next(256), (byte)Random.Shared.Next(256), (byte)Random.Shared.Next(256));

            chartEntries.Add(new ChartEntry(pair.Value)
            {
                Label = pair.Key,
                ValueLabel = pair.Value.ToString(),
                Color = color
            });
        }

        // If no entries, show a default chart
        if (chartEntries.Count == 0)
        {
            chartEntries.Add(new ChartEntry(0)
            {
                Label = "No moods",
                ValueLabel = "0",
                Color = new SKColor(200, 200, 200)
            });
        }

        // Update our class field with the new entries
        entries = chartEntries.ToArray();

        // Update the chart with new entries
        Chart.Chart = new DonutChart
        {
            Entries = entries,
            LabelTextSize = 30,
            ValueLabelOrientation = Orientation.Horizontal,
            LabelOrientation = Orientation.Horizontal,
            AnimationDuration = TimeSpan.FromSeconds(1)
        };
    }

    private async void DeleteMoodEntry(MoodEntry_VM moodEntry)
    {
        // Confirm deletion
        bool confirmed = await Shell.Current.DisplayAlert("Delete Mood Entry",
            $"Are you sure you want to delete the mood entry for {moodEntry.Mood} on {moodEntry.Date.ToShortDateString()}?",
            "Yes", "No");

        if (confirmed)
        {
            // Remove the mood entry from the service
            _moodEntryService.MoodEntries.Remove(moodEntry);

            await Shell.Current.DisplayAlert("Mood Entry Deleted",
                $"The mood entry for {moodEntry.Mood} on {moodEntry.Date.ToShortDateString()} has been deleted.", "OK");

            // The chart will update automatically through the collection changed event
        }
    }

    private async void AddMood_Clicked(object sender, EventArgs e)
    {
        var popup = new AddMood_PopUp();
        var result = await PopupService.ShowPopupAsync(popup);

        if (result is MoodEntry_VM moodEntry)
        {
            // Display the alert
            await Shell.Current.DisplayAlert("New Mood Entry",
                $"Mood: {moodEntry.Mood}\nDate: {moodEntry.Date.ToShortDateString()}", "OK");

            // Add the mood entry to the service
            _moodEntryService.AddMoodEntry(moodEntry);

            // The chart will update automatically through the collection changed event
        }
    }
}