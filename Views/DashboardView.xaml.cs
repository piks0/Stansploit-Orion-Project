using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using StansploitOrionProject.Services;

namespace StansploitOrionProject.Views;

public partial class DashboardView : UserControl, IDisposable
{
    private readonly SystemInfoService _sys = new();
    private readonly DispatcherTimer _timer = new();
    private CancellationTokenSource? _loadCancellationTokenSource;

    public DashboardView()
    {
        InitializeComponent();
        _ = LoadInfoAsync();

        _timer.Interval = TimeSpan.FromSeconds(2);
        _timer.Tick += Timer_Tick;
        _timer.Start();
    }

    private async Task LoadInfoAsync()
    {
        _loadCancellationTokenSource?.Cancel();
        _loadCancellationTokenSource = new CancellationTokenSource();
        var token = _loadCancellationTokenSource.Token;

        try
        {
            var cpuTask = _sys.GetCpuAsync();
            var gpuTask = _sys.GetGpuAsync();
            var osTask = _sys.GetOsAsync();
            var ramTask = _sys.GetRamAsync();

            if (token.IsCancellationRequested) return;

            CpuHardwareValue.Text = await cpuTask;
            GpuHardwareValue.Text = await gpuTask;
            RamHardwareValue.Text = await ramTask;
            OsHardwareValue.Text = await osTask;
            PowerText.Text = _sys.GetActivePowerPlan();
            UptimeText.Text = _sys.GetUptime();
            CpuStatusText.Text = await cpuTask;
            GpuStatusText.Text = await gpuTask;
            RamStatusText.Text = await ramTask;
            OsStatusText.Text = await osTask;
            PowerStatusText.Text = _sys.GetActivePowerPlan();
        }
        catch (OperationCanceledException)
        {
            // A newer refresh superseded this one.
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Dashboard load failed: {ex}");
            SetPlaceholderValues();
        }
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        try
        {
            float cpu = _sys.GetCpuUsage();
            float gpu = _sys.GetGpuUsage();
            float disk = _sys.GetDiskUsage();
            var net = _sys.GetNetworkUsage();
            var (used, total, ramPercent) = _sys.GetRamUsage();

            CpuUsageText.Text = $"{cpu:F1}%";
            CpuUsageProgress.Value = Math.Min(100, Math.Max(0, cpu));
            GpuUsageText.Text = gpu < 0 ? "N/A" : $"{gpu:F1}%";
            GpuUsageProgress.Value = gpu < 0 ? 0 : Math.Min(100, Math.Max(0, gpu));
            RamUsageText.Text = $"{used:F1} / {total:F1} GB";
            RamUsageProgress.Value = Math.Min(100, Math.Max(0, ramPercent));
            DiskUsageText.Text = $"{disk:F1}%";
            DiskUsageProgress.Value = Math.Min(100, Math.Max(0, disk));
            NetUsageText.Text = $"↓ {net.downloadKbps:F1} KB/s\n↑ {net.uploadKbps:F1} KB/s";
            NetworkUsageProgress.Value = Math.Min(100, Math.Max(0, (net.downloadKbps + net.uploadKbps) / 2));
            UptimeText.Text = _sys.GetUptime();
            PowerText.Text = _sys.GetActivePowerPlan();
            CpuStatusText.Text = $"{cpu:F1}%";
            GpuStatusText.Text = gpu < 0 ? "N/A" : $"{gpu:F1}%";
            RamStatusText.Text = $"{used:F1} / {total:F1} GB";
            PowerStatusText.Text = _sys.GetActivePowerPlan();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Dashboard refresh failed: {ex}");
            SetPlaceholderValues();
        }
    }

    private async void ScanSystem_Click(object sender, RoutedEventArgs e)
    {
        await LoadInfoAsync();
    }

    private void OptimizeNow_Click(object sender, RoutedEventArgs e)
    {
        var window = Window.GetWindow(this) as MainWindow;
        window?.NavigateTo("Tweaks");
    }

    private void GoToTweaks_Click(object sender, RoutedEventArgs e)
    {
        var window = Window.GetWindow(this) as MainWindow;
        window?.NavigateTo("Tweaks");
    }

    private void GoToInstaller_Click(object sender, RoutedEventArgs e)
    {
        var window = Window.GetWindow(this) as MainWindow;
        window?.NavigateTo("Installer");
    }

    private void OpenTaskManager_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "taskmgr.exe",
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to open Task Manager: {ex}");
        }
    }

    private void SetPlaceholderValues()
    {
        CpuHardwareValue.Text = "N/A";
        GpuHardwareValue.Text = "N/A";
        RamHardwareValue.Text = "N/A";
        OsHardwareValue.Text = "N/A";
        CpuStatusText.Text = "N/A";
        GpuStatusText.Text = "N/A";
        RamStatusText.Text = "N/A";
        OsStatusText.Text = "N/A";
        PowerText.Text = "N/A";
        PowerStatusText.Text = "N/A";
        UptimeText.Text = "N/A";
        CpuUsageText.Text = "N/A";
        GpuUsageText.Text = "N/A";
        RamUsageText.Text = "N/A";
        DiskUsageText.Text = "N/A";
        NetUsageText.Text = "N/A";
    }

    public void Dispose()
    {
        _loadCancellationTokenSource?.Cancel();
        _timer.Stop();
        _timer.Tick -= Timer_Tick;
        _sys.Dispose();
    }
}