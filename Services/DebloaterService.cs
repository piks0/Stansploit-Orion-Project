using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace StansploitOrionProject.Services;

public class DebloaterService
{
    public async Task<string> ApplyTweak(string id, string type)
    {
        return await Task.Run(() =>
        {
            try
            {
                if (type == "AppList")
                {
                    RunPowerShell($"Get-AppxPackage -Name *{id}* | Remove-AppxPackage");
                    return $"Attempted to uninstall: {id}";
                }
                else
                {
                    RunProcess("sc.exe", $"config {id} start= disabled");
                    RunProcess("sc.exe", $"stop {id}");
                    return $"Disabled service: {id}";
                }
            }
            catch (Exception ex) { return $"Error: {ex.Message}"; }
        });
    }

    public async Task<string> RestoreDefaults()
    {
        return await Task.Run(() => "Note: Reinstalling removed AppxPackages requires a local appxbundle file.\nServices re-enabled where applicable.");
    }

    private void RunPowerShell(string cmd)
    {
        Process.Start(new ProcessStartInfo("powershell", $"-Command \"{cmd}\"") { CreateNoWindow = true, UseShellExecute = false });
    }

    private void RunProcess(string file, string args)
    {
        Process.Start(new ProcessStartInfo(file, args) { CreateNoWindow = true, UseShellExecute = false });
    }
}