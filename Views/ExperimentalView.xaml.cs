using System.Windows;
using System.Windows.Controls;
using StansploitOrionProject.Services;

namespace StansploitOrionProject.Views;

public partial class ExperimentalView : UserControl
{
    private readonly ExperimentalService _service = new();

    public ExperimentalView() => InitializeComponent();

    private void Log(string msg) => LogOutput.Text += $"\n{System.DateTime.Now:HH:mm:ss} - {msg}";

    private void Tweak_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is string action)
        {
            var result = MessageBox.Show($"Confirm action: {action}?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                Log(_service.ExecuteTweak(action));
            }
        }
    }
}