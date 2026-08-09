using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using StansploitOrionProject.Services;

namespace StansploitOrionProject.Views;

public partial class InstallerView : UserControl
{
    private readonly InstallerService _service;

    public InstallerView()
    {
        InitializeComponent();
        _service = new InstallerService();
    }

    private void Log(string message)
    {
        LogOutput.Text += $"\n{System.DateTime.Now:HH:mm:ss} - {message}";
    }

    private async void Install_Click(object sender, RoutedEventArgs e)
    {
        var selectedPackages = new List<string>();
        foreach (var child in PackageList.Children)
        {
            if (child is CheckBox cb && cb.IsChecked == true && cb.Tag is string id)
            {
                selectedPackages.Add(id);
            }
        }

        if (selectedPackages.Count == 0)
        {
            Log("No packages selected.");
            return;
        }

        foreach (var id in selectedPackages)
        {
            Log($"Starting installation of {id}...");
            string result = await _service.InstallPackageAsync(id);
            Log(result);
        }
        Log("Batch process finished.");
    }
}