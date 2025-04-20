using THEMOOD.Controls;
using THEMOOD.PopUps;
using CommunityToolkit.Maui.Views;
using THEMOOD.Services;
using System.Threading.Tasks;
using THEMOOD.ViewModels;

namespace THEMOOD.Pages;

public partial class MoodEntryPage : ContentView
{
    public MoodEntryPage()
    {
        InitializeComponent();
    }

    private async void AddMood_Clicked(object sender, EventArgs e)
    {
        var popup = new AddMood_PopUp();
        var result = await PopupService.ShowPopupAsync(popup);

        if (result is MoodEntry_VM moodEntry)
        {
            // Do something with the mood entry, for example:
            // Add to your collection or save to database
            await Shell.Current.DisplayAlert("New Mood Entry", $"Mood: {moodEntry.Mood}\nDate: {moodEntry.Date.ToShortDateString()}", "OK");


            // If you have a view model with a collection of mood entries:
            // ViewModel.MoodEntries.Add(moodEntry);

            // Or if using a service to save entries:
            // await DataService.SaveMoodEntryAsync(moodEntry);
        }
    }
}