using System.Windows;
using System.Windows.Controls;
using StansploitOrionProject.Services;

namespace StansploitOrionProject.Views;

public partial class PowerPlanView : UserControl
{
    private readonly PowerPlanService _service;

    public PowerPlanView()
    {
        InitializeComponent();
        _service = new PowerPlanService();
        UpdateStatus();
    }

    private void UpdateStatus()
    {
        CurrentPlanText.Text = _service.GetCurrentPlanName();
    }

    private void Log(string message)
    {
        LogOutput.Text += $"\n{System.DateTime.Now:HH:mm:ss} - {message}";
    }

    private void ApplyPlan_Click(object sender, RoutedEventArgs e)
    {
        Log("Applying Orion Gaming Plan...");
        if (_service.ApplyOrionPlan(out string msg))
        {
            Log("Success: " + msg);
        }
        else
        {
            Log("Error: " + msg);
        }
        UpdateStatus();
    }

    private void RestoreDefaults_Click(object sender, RoutedEventArgs e)
    {
        Log("Restoring previous plan...");
        if (_service.RestoreDefaults(out string msg))
        {
            Log("Success: " + msg);
        }
        else
        {
            Log("Error: " + msg);
        }
        UpdateStatus();
    }
}