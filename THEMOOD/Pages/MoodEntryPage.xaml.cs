using THEMOOD.Controls;
using THEMOOD.PopUps;
using CommunityToolkit.Maui.Views;
using THEMOOD.Services;
using System.Threading.Tasks;
using THEMOOD.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace THEMOOD.Pages;

public partial class MoodEntryPage : ContentView
{
    // Reference the singleton service
    private MoodEntryService _moodEntryService = MoodEntryService.Instance;

    // Add the DeleteCommand property declaration
    public ICommand DeleteCommand { get; private set; }

    public MoodEntryPage()
    {
        InitializeComponent();

        // Initialize the DeleteCommand
        DeleteCommand = new Command<MoodEntry_VM>(DeleteMoodEntry);

        // Bind the CollectionView to the service's ObservableCollection
        // Use MoodLog instead of MoodEntryLog since that's the name in your XAML
        MoodLog.ItemsSource = _moodEntryService.MoodEntries;
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

            // No need to update ItemsSource, since it's bound to the ObservableCollection
            // that automatically notifies of changes
        }
    }
}