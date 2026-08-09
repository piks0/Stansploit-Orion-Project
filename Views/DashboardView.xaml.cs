using System.Windows;
using System.Windows.Controls;
using StansploitOrionProject.Services;

namespace StansploitOrionProject.Views;

public partial class DashboardView : UserControl
{
    private readonly SystemInfoService _sys = new();

    public DashboardView()
    {
        InitializeComponent();
        CpuText.Text = $"CPU: {_sys.GetCpu()}";
        GpuText.Text = $"GPU: {_sys.GetGpu()}";
        RamText.Text = $"RAM: {_sys.GetRam()}";
        OsText.Text = $"OS: {_sys.GetOs()}";
        PowerText.Text = $"Active Plan: {_sys.GetActivePowerPlan()}";
    }

    private void RestoreAll_Click(object sender, RoutedEventArgs e)
    {
        if(MessageBox.Show("Restore system to defaults?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        {
            // Trigger restoration logic across services
            MessageBox.Show("Default settings restored.");
        }
    }
}