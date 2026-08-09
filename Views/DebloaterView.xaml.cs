using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using StansploitOrionProject.Services;

namespace StansploitOrionProject.Views;

public partial class DebloaterView : UserControl
{
    private readonly DebloaterService _service;

    public DebloaterView()
    {
        InitializeComponent();
        _service = new DebloaterService();
    }

    private void Log(string message) => LogOutput.Text += $"\n{System.DateTime.Now:HH:mm:ss} - {message}";

    private void SelectSafe_Click(object sender, RoutedEventArgs e)
    {
        foreach (var group in new[] { AppList, ServiceList })
            foreach (var child in group.Children)
                if (child is CheckBox cb) cb.IsChecked = true;
        Log("Safe preset selected.");
    }

    private async void Apply_Click(object sender, RoutedEventArgs e)
    {
        Log("Applying optimizations...");
        foreach (var group in new[] { AppList, ServiceList })
            foreach (var child in group.Children)
                if (child is CheckBox { IsChecked: true, Tag: string tag })
                    Log(await _service.ApplyTweak(tag, group.Name));
    }

    private async void Restore_Click(object sender, RoutedEventArgs e)
    {
        Log("Restoring defaults...");
        Log(await _service.RestoreDefaults());
    }
}