using THEMOOD.Controls;
using THEMOOD.PopUps;
using CommunityToolkit.Maui.Views;
using THEMOOD.Services;
using System.Threading.Tasks;
using THEMOOD.ViewModels;
using System.Collections.ObjectModel;

namespace THEMOOD.Pages;

public partial class MoodEntryPage : ContentView
{
    ObservableCollection<MoodEntry_VM> MoodEntries;
    public MoodEntryPage()
    {
        InitializeComponent();
        MoodEntries = new ObservableCollection<MoodEntry_VM>();
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
            var moodlog = (MoodEntry_VM)result;

            switch (moodlog.Mood)
            {
                case "Angry":
                    moodlog.MoodIcon = "😠";
                    break;
                case "Anxious":
                    moodlog.MoodIcon = "😰";
                    break;
                case "Bored":
                    moodlog.MoodIcon = "😐";
                    break;
                case "Calm":
                    moodlog.MoodIcon = "😌";
                    break;
                case "Content":
                    moodlog.MoodIcon = "😊";
                    break;
                case "Depressed":
                    moodlog.MoodIcon = "😞";
                    break;
                case "Envious":
                    moodlog.MoodIcon = "😒";
                    break;
                case "Grateful":
                    moodlog.MoodIcon = "🙏";
                    break;
                case "Guilty":
                    moodlog.MoodIcon = "😔";
                    break;
                case "Happy":
                    moodlog.MoodIcon = "😄";
                    break;
                case "Hopeful":
                    moodlog.MoodIcon = "🌈";
                    break;
                case "Irritated":
                    moodlog.MoodIcon = "😤";
                    break;
                case "Lonely":
                    moodlog.MoodIcon = "😢";
                    break;
                case "Loving":
                    moodlog.MoodIcon = "❤️";
                    break;
                case "Neutral":
                    moodlog.MoodIcon = "😶";
                    break;
                case "Optimistic":
                    moodlog.MoodIcon = "🤞";
                    break;
                case "Pleased":
                    moodlog.MoodIcon = "😁";
                    break;
                case "Sad":
                    moodlog.MoodIcon = "😢";
                    break;
                case "Stressed":
                    moodlog.MoodIcon = "😫";
                    break;
                default:
                    moodlog.MoodIcon = "❓"; // fallback icon for unrecognized mood
                    break;
            }


            MoodEntries.Add(moodlog);

            MoodLog.ItemsSource = MoodEntries;

            // If you have a view model with a collection of mood entries:
            // ViewModel.MoodEntries.Add(moodEntry);

            // Or if using a service to save entries:
            // await DataService.SaveMoodEntryAsync(moodEntry);
        }
    }
}