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
        MoodEntry_VM entry = new MoodEntry_VM
        {
            Date = MoodDatePicker.Date,
            Mood = MoodPicker.SelectedItem?.ToString(),
            Note = MoodNotesEntry.Text // Changed to match the VM property name and XAML control name
        };

        Close(entry);
    }
}
