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
        { "Angry", new SKColor(244, 67, 54) },       // Red - intense and aggressive
        { "Anxious", new SKColor(156, 39, 176) },    // Purple - tension and unease
        { "Bored", new SKColor(121, 85, 72) },       // Brown - dull, lack of excitement
        { "Calm", new SKColor(129, 212, 250) },      // Light Blue - peaceful and gentle
        { "Content", new SKColor(76, 175, 80) },     // Green - harmony and satisfaction
        { "Depressed", new SKColor(33, 33, 33) },    // Very dark gray - heavy and hopeless
        { "Envious", new SKColor(139, 195, 74) },    // Lime Green - jealousy, envy
        { "Grateful", new SKColor(255, 215, 0) },    // Gold - warmth and appreciation
        { "Guilty", new SKColor(96, 125, 139) },     // Blue Gray - mixed emotions
        { "Happy", new SKColor(255, 193, 7) },       // Amber - bright and cheerful
        { "Hopeful", new SKColor(0, 188, 212) },     // Cyan - uplifting, future-looking
        { "Irritated", new SKColor(255, 87, 34) },   // Deep Orange - agitation
        { "Lonely", new SKColor(103, 58, 183) },     // Deep Purple - emotional depth, isolation
        { "Loving", new SKColor(233, 30, 99) },      // Pink - affectionate, passionate
        { "Neutral", new SKColor(158, 158, 158) },   // Gray - balance, indifference
        { "Optimistic", new SKColor(255, 235, 59) }, // Bright Yellow - hope and brightness
        { "Pleased", new SKColor(255, 152, 0) },     // Orange - satisfaction and energy
        { "Sad", new SKColor(33, 150, 243) },        // Blue - melancholy
        { "Stressed", new SKColor(198, 40, 40) }    // Dark Red - high pressure, intensity
    };

    public MoodEntryPage()
    {
        InitializeComponent();

        // Initialize the DeleteCommand
        DeleteCommand = new Command<MoodEntry_VM>(DeleteMoodEntry);

        // Bind the CollectionView to the service's ObservableCollection
        MoodLog.ItemsSource = _moodEntryService.MoodEntries;

        // Subscribe to collection changes to update chart and analysis
        if (_moodEntryService.MoodEntries is ObservableCollection<MoodEntry_VM> observableMoodEntries)
        {
            observableMoodEntries.CollectionChanged += MoodEntries_CollectionChanged;
        }

        // Initial chart update
        UpdateMoodChart();

        // Initial mood analysis
        UpdateMoodAnalysis();
    }

    private void MoodEntries_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        // Update the chart when the collection changes
        UpdateMoodChart();

        // Update the mood analysis when the collection changes
        UpdateMoodAnalysis();
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
        Chart.Chart = new BarChart
        {
            Entries = entries,
            LabelTextSize = 30,
            ValueLabelOrientation = Orientation.Horizontal,
            LabelOrientation = Orientation.Horizontal,
            AnimationDuration = TimeSpan.FromSeconds(1)
        };
    }

    // New method to update mood analysis
    private async void UpdateMoodAnalysis()
    {
        try
        {
            if (_moodEntryService.MoodEntries.Count < 3)
            {
                AnalysisLabel.Text = "AI Analysis on your mood will show after entering at least 3 moods";
                return;
            }

            // Show loading indicator
            AnalysisLabel.Text = "Analyzing your mood patterns...";

            // Get the analysis from the service
            string analysis = await _moodEntryService.GetMoodAnalysis();

            // Update the UI with the analysis
            AnalysisLabel.Text = analysis;
        }
        catch (Exception ex)
        {
            AnalysisLabel.Text = "Unable to analyze moods at this time.";
            Console.WriteLine($"Error analyzing moods: {ex.Message}");
        }
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

            // The chart and analysis will update automatically through the collection changed event
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

            // The chart and analysis will update automatically through the collection changed event
        }
    }
}