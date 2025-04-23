using CommunityToolkit.Maui.Views;
using THEMOOD.ViewModels;
using THEMOOD.Services;

namespace THEMOOD.PopUps;

public partial class AddMood_PopUp : Popup
{
    public AddMood_PopUp()
    {
        InitializeComponent();
    }

    private void CancelButton_Clicked(object sender, EventArgs e)
    {
        Close();
    }

    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        // Extract mood without emoji first
        string moodText = string.Empty;
        var selected = MoodPicker.SelectedItem?.ToString();
        if (!string.IsNullOrWhiteSpace(selected))
        {
            var parts = selected.Split(' ', 2);
            moodText = parts.Length > 1 ? parts[1] : selected;
        }

        // Now create the entry
        MoodEntry_VM entry = new MoodEntry_VM
        {
            Mood = moodText,
            Date = DateOnly.FromDateTime(MoodDatePicker.Date),
            Note = MoodNotesEntry.Text
        };


        Close(entry);
    }
}
