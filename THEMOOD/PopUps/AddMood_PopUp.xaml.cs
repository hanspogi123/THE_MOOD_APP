using CommunityToolkit.Maui.Views;
using THEMOOD.ViewModels;
using THEMOOD.Services;
using THEMOOD.Converters;

namespace THEMOOD.PopUps;

public partial class AddMood_PopUp : Popup
{
    private bool _isSavedEnabled;
    public bool IsSavedEnabled
    {
        get => _isSavedEnabled;
        set
        {
            if (_isSavedEnabled != value)
            {
                _isSavedEnabled = value;
                OnPropertyChanged(nameof(IsSavedEnabled));
            }
        }
    }

    public AddMood_PopUp()
    {
        InitializeComponent();

        IsSavedEnabled = false;
        SaveButton.IsEnabled = false;

        SaveButton.BindingContext = this;

        MoodDatePicker.MaximumDate = DateTime.Today;

        MoodDatePicker.DateSelected += ValidateForm;
        MoodPicker.SelectedIndexChanged += ValidateForm;
        MoodNotesEntry.TextChanged += ValidateForm;
    }

    public void ValidateForm(object sender, EventArgs e)
    {
        ValidateForm();
    }

    private void ValidateForm()
    {
        // Check if all fields are filled
        bool isDateSelected = MoodDatePicker.Date != DateTime.MinValue;
        bool isMoodSelected = MoodPicker.SelectedIndex != -1;
        bool isNoteEntered = !string.IsNullOrWhiteSpace(MoodNotesEntry.Text);
        bool isValidDate = MoodDatePicker.Date <= DateTime.Today;

        // Enable Save button if all fields are filled
        IsSavedEnabled = isDateSelected && isMoodSelected && isNoteEntered;
        SaveButton.IsEnabled = IsSavedEnabled;
    }

    private void CancelButton_Clicked(object sender, EventArgs e)
    {
        Close();
    }

    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        if (!IsSavedEnabled)
            return;

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