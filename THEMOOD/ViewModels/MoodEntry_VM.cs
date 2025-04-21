using CommunityToolkit.Mvvm.ComponentModel;

namespace THEMOOD.ViewModels
{
    public partial class MoodEntry_VM : ObservableObject
    {
        [ObservableProperty]
        private DateTime date;

        [ObservableProperty]
        private string mood;

        [ObservableProperty]
        private string note;

        [ObservableProperty]
        private string moodIcon;
    }
}