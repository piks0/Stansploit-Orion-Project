using System;
using System.Management;
using System.Diagnostics;
using System.Threading.Tasks;

namespace StansploitOrionProject.Services;

public class SystemInfoService : IDisposable
{
    private PerformanceCounter? _cpuCounter;
    private PerformanceCounter? _ramCounter;
    private PerformanceCounter? _diskCounter;
    private PerformanceCounter? _netReadCounter;
    private PerformanceCounter? _netWriteCounter;
    private PerformanceCounter? _gpuCounter;

    private double _totalRamGb = 0;
    private bool _disposed = false;
    private readonly Task _initTask;

    public SystemInfoService()
    {
        _initTask = Task.Run(() =>
        {
            var sw = Stopwatch.StartNew();
            try
            {
                _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total", true);
                _cpuCounter.NextValue();
            }
            catch (Exception ex) { Debug.WriteLine($"CPU Counter Init Failed: {ex.Message}"); }

            try
            {
                _ramCounter = new PerformanceCounter("Memory", "Available MBytes", true);
                _ramCounter.NextValue();
            }
            catch (Exception ex) { Debug.WriteLine($"RAM Counter Init Failed: {ex.Message}"); }

            try
            {
                _diskCounter = new PerformanceCounter("LogicalDisk", "% Disk Time", "_Total", true);
                _diskCounter.NextValue();
            }
            catch (Exception ex) { Debug.WriteLine($"Disk Counter Init Failed: {ex.Message}"); }

            try
            {
                var category = new PerformanceCounterCategory("Network Interface");
                var instances = category.GetInstanceNames();
                if (instances.Length > 0)
                {
                    string instance = instances[0];
                    _netReadCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", instance, true);
                    _netWriteCounter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", instance, true);
                    _netReadCounter.NextValue();
                    _netWriteCounter.NextValue();
                }
            }
            catch (Exception ex) { Debug.WriteLine($"Network Counter Init Failed: {ex.Message}"); }

            try
            {
                var gpuCategory = new PerformanceCounterCategory("GPU Engine");
                var instanceNames = gpuCategory.GetInstanceNames();
                if (instanceNames.Length > 0)
                {
                    _gpuCounter = new PerformanceCounter("GPU Engine", "Utilization Percentage", instanceNames[0], true);
                    _gpuCounter.NextValue();
                }
            }
            catch (Exception ex) { Debug.WriteLine($"GPU Counter Init Failed: {ex.Message}"); }

            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT TotalPhysicalMemory FROM Win32_ComputerSystem");
                foreach (var obj in searcher.Get())
                {
                    var val = obj["TotalPhysicalMemory"];
                    if (val != null && ulong.TryParse(val.ToString(), out ulong bytes))
                    {
                        _totalRamGb = bytes / 1073741824.0;
                        break;
                    }
                }
            }
            catch (Exception ex) { Debug.WriteLine($"RAM WMI Init Failed: {ex.Message}"); }

            if (_totalRamGb <= 0) _totalRamGb = 16.0;

            sw.Stop();
            Debug.WriteLine($"SystemInfoService Init completed in {sw.ElapsedMilliseconds}ms (Total RAM: {_totalRamGb} GB)");
        });
    }

    public async Task<string> GetCpuAsync() => await Task.Run(() => GetWmiValue("Win32_Processor", "Name"));
    public async Task<string> GetGpuAsync() => await Task.Run(() => GetWmiValue("Win32_VideoController", "Name"));
    public async Task<string> GetOsAsync() => await Task.Run(() => GetWmiValue("Win32_OperatingSystem", "Caption"));
    
    public async Task<string> GetRamAsync()
    {
        await _initTask;
        return _totalRamGb.ToString("F2") + " GB";
    }

    
    public float GetCpuUsage()
    {
        if (_disposed || _cpuCounter == null) return 0f;
        try { return _cpuCounter.NextValue(); }
        catch { return 0f; }
    }

    public float GetGpuUsage()
    {
        if (_disposed || _gpuCounter == null) return -1f; // -1 means N/A
        try { return _gpuCounter.NextValue(); }
        catch { return -1f; }
    }

    public float GetDiskUsage()
    {
        if (_disposed || _diskCounter == null) return 0f;
        try { return _diskCounter.NextValue(); }
        catch { return 0f; }
    }

    public (double downloadKbps, double uploadKbps) GetNetworkUsage()
    {
        if (_disposed) return (0, 0);
        try
        {
            float rxBytesSec = _netReadCounter?.NextValue() ?? 0f;
            float txBytesSec = _netWriteCounter?.NextValue() ?? 0f;
            return (rxBytesSec / 1024.0, txBytesSec / 1024.0); // KB/s
        }
        catch { return (0, 0); }
    }

    public string GetUptime()
    {
        try
        {
            long tickCount = Environment.TickCount64;
            TimeSpan uptime = TimeSpan.FromMilliseconds(tickCount);
            if (uptime.Days > 0)
            {
                return $"{uptime.Days}d {uptime.Hours}h {uptime.Minutes}m {uptime.Seconds}s";
            }
            return $"{uptime.Hours:D2}:{uptime.Minutes:D2}:{uptime.Seconds:D2}";
        }
        catch
        {
            return "N/A";
        }
    }

    public (double usedGb, double totalGb, float usagePercent) GetRamUsage()
    {
        if (_disposed || _ramCounter == null) return (0, _totalRamGb, 0f);
        try
        {
            double availMb = _ramCounter.NextValue();
            double availGb = availMb / 1024.0;
            double usedGb = Math.Max(0, _totalRamGb - availGb);
            float percent = (float)((usedGb / _totalRamGb) * 100.0);
            return (usedGb, _totalRamGb, Math.Clamp(percent, 0f, 100f));
        }
        catch { return (0, _totalRamGb, 0f); }
    }

    public string GetActivePowerPlan()
    {
        try
        {
            Process p = new Process { StartInfo = { FileName = "powercfg", Arguments = "-getactivescheme", UseShellExecute = false, RedirectStandardOutput = true, CreateNoWindow = true } };
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            if (output.Contains('(') && output.Contains(')'))
            {
                return output.Split('(')[1].TrimEnd(')');
            }
            return output.Trim();
        }
        catch { return "Balanced"; }
    }

    private string GetWmiValue(string table, string column)
    {
        try {
            using var searcher = new ManagementObjectSearcher($"SELECT {column} FROM {table}");
            foreach (var obj in searcher.Get()) 
            {
                var val = obj[column]?.ToString();
                if (val != null) return val;
            }
        } catch { }
        return "N/A";
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _cpuCounter?.Dispose();
            _ramCounter?.Dispose();
            _diskCounter?.Dispose();
            _netReadCounter?.Dispose();
            _netWriteCounter?.Dispose();
            _gpuCounter?.Dispose();
            _disposed = true;
        }
    }
}


