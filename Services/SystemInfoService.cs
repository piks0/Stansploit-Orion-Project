using System.Management;
using System.Diagnostics;

namespace StansploitOrionProject.Services;

public class SystemInfoService
{
    public string GetCpu() => GetWmiValue("Win32_Processor", "Name");
    public string GetGpu() => GetWmiValue("Win32_VideoController", "Name");
    public string GetRam() => (Convert.ToDouble(GetWmiValue("Win32_ComputerSystem", "TotalPhysicalMemory")) / 1073741824).ToString("F2") + " GB";
    public string GetOs() => GetWmiValue("Win32_OperatingSystem", "Caption");
    
    public string GetActivePowerPlan()
    {
        Process p = new Process { StartInfo = { FileName = "powercfg", Arguments = "-getactivescheme", UseShellExecute = false, RedirectStandardOutput = true, CreateNoWindow = true } };
        p.Start();
        return p.StandardOutput.ReadToEnd().Split('(')[1].TrimEnd(')');
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
}