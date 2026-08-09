using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using StansploitOrionProject.Services;

namespace StansploitOrionProject.Views;

public partial class DashboardView : UserControl, IDisposable
{
    private readonly SystemInfoService _sys = new();
    private readonly DispatcherTimer _timer = new();

    public DashboardView()
    {
        InitializeComponent();
        LoadInfoAsync();
        
        _timer.Interval = TimeSpan.FromSeconds(2);
        _timer.Tick += Timer_Tick;
        _timer.Start();
    }

    private async void LoadInfoAsync()
    {
        var cpuTask = _sys.GetCpuAsync();
        var gpuTask = _sys.GetGpuAsync();
        var osTask = _sys.GetOsAsync();

        StaticCpuText.Text = $"CPU: {await cpuTask}";
        StaticGpuText.Text = $"GPU: {await gpuTask}";
        StaticRamText.Text = $"Total RAM: {await _sys.GetRamAsync()}";
        StaticOsText.Text = $"Operating System: {await osTask}";
        PowerText.Text = $"Active Power Plan: {_sys.GetActivePowerPlan()}";
        UptimeText.Text = $"Uptime: {_sys.GetUptime()}";
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        float cpu = _sys.GetCpuUsage();
        float gpu = _sys.GetGpuUsage();
        float disk = _sys.GetDiskUsage();
        var net = _sys.GetNetworkUsage();
        var (used, total, ramPercent) = _sys.GetRamUsage();
        
        CpuUsageText.Text = $"CPU Usage: {cpu:F1}%";
        GpuUsageText.Text = $"GPU Usage: {(gpu < 0 ? "N/A" : gpu.ToString("F1") + "%")}";
        RamUsageText.Text = $"RAM Usage: {used:F2} / {total:F2} GB ({ramPercent:F1}%)";
        DiskUsageText.Text = $"Disk Usage: {disk:F1}%";
        NetUsageText.Text = $"Network: Down: {net.downloadKbps:F1} KB/s | Up: {net.uploadKbps:F1} KB/s";
        UptimeText.Text = $"Uptime: {_sys.GetUptime()}";
    }

    private void GoToTweaks_Click(object sender, RoutedEventArgs e)
    {
        if (Window.GetWindow(this) is MainWindow mw)
        {
            // We need to access LoadView, which is in MainWindow. 
            // Since LoadView is private, we can either make it public or 
            // use a public property/method on MainWindow. 
            // Let's use a public method to navigate.
            mw.NavigateTo("Tweaks");
        }
    }

    private void GoToInstaller_Click(object sender, RoutedEventArgs e)
    {
        if (Window.GetWindow(this) is MainWindow mw)
        {
            mw.NavigateTo("Installer");
        }
    }

    private void OpenTaskManager_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Process.Start("taskmgr.exe");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Could not open Task Manager: {ex.Message}");
        }
    }


    public void Dispose()
    {
        _timer.Stop();
        _sys.Dispose();
    }
}
