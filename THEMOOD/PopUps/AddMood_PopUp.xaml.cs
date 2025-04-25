using CommunityToolkit.Maui.Views;
using THEMOOD.ViewModels;
using THEMOOD.Services;
using THEMOOD.Converters;
using Microcharts.Maui;
using Microcharts;
using SkiaSharp; // Add this import for SKColor

namespace THEMOOD.PopUps;

public partial class AddMood_PopUp : Popup
{
    // Make entries a class-level field
    private ChartEntry[] entries;

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

        // Initialize entries as a class field
        entries = new[]
        {
            new ChartEntry(5)
            {
                Label = "Happy",
                ValueLabel = "5",
                // Convert MAUI Color to SKColor
                Color = new SKColor(255, 0, 0) // Red
            },
            new ChartEntry(3)
            {
                Label = "Sad",
                ValueLabel = "3",
                Color = new SKColor(0, 255, 0) // Green
            },
            new ChartEntry(8)
            {
                Label = "Neutral",
                ValueLabel = "8",
                Color = new SKColor(0, 0, 255) // Blue
            }
        };

        // Assign the chart to the ChartView
        Chart.Chart = new BarChart
        {
            Entries = entries,
            LabelTextSize = 40,
            ValueLabelOrientation = Orientation.Horizontal,
            LabelOrientation = Orientation.Horizontal,
            AnimationDuration = TimeSpan.FromSeconds(1)
        };

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

        if (MoodPicker.SelectedIndex != -1)
        {
            // Get selected mood
            string selectedMood = MoodPicker.SelectedItem.ToString();

            // Update chart accordingly
            UpdateChartWithSelectedMood(selectedMood);
        }
    }

    private void UpdateChartWithSelectedMood(string selectedMood)
    {
        // Create new ChartEntry array with updated values
        ChartEntry[] updatedEntries;

        if (selectedMood.Contains("Happy"))
        {
            updatedEntries = new[]
            {
            new ChartEntry(10) { Label = "Happy", ValueLabel = "10", Color = new SKColor(255, 0, 0) },
            new ChartEntry(2) { Label = "Sad", ValueLabel = "2", Color = new SKColor(0, 255, 0) },
            new ChartEntry(5) { Label = "Neutral", ValueLabel = "5", Color = new SKColor(0, 0, 255) }
        };
        }
        else if (selectedMood.Contains("Sad"))
        {
            updatedEntries = new[]
            {
            new ChartEntry(2) { Label = "Happy", ValueLabel = "2", Color = new SKColor(255, 0, 0) },
            new ChartEntry(10) { Label = "Sad", ValueLabel = "10", Color = new SKColor(0, 255, 0) },
            new ChartEntry(3) { Label = "Neutral", ValueLabel = "3", Color = new SKColor(0, 0, 255) }
        };
        }
        else if (selectedMood.Contains("Neutral"))
        {
            updatedEntries = new[]
            {
            new ChartEntry(5) { Label = "Happy", ValueLabel = "5", Color = new SKColor(255, 0, 0) },
            new ChartEntry(5) { Label = "Sad", ValueLabel = "5", Color = new SKColor(0, 255, 0) },
            new ChartEntry(10) { Label = "Neutral", ValueLabel = "10", Color = new SKColor(0, 0, 255) }
        };
        }
        else
        {
            // Default values
            updatedEntries = new[]
            {
            new ChartEntry(5) { Label = "Happy", ValueLabel = "5", Color = new SKColor(255, 0, 0) },
            new ChartEntry(5) { Label = "Sad", ValueLabel = "5", Color = new SKColor(0, 255, 0) },
            new ChartEntry(5) { Label = "Neutral", ValueLabel = "5", Color = new SKColor(0, 0, 255) }
        };
        }

        // Update our class field to the new entries
        entries = updatedEntries;

        // Update the chart with new entries
        Chart.Chart = new BarChart
        {
            Entries = entries,
            LabelTextSize = 40,
            ValueLabelOrientation = Orientation.Horizontal,
            LabelOrientation = Orientation.Horizontal
        };
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