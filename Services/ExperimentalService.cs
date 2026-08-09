using Microsoft.Win32;

namespace StansploitOrionProject.Services;

public class ExperimentalService
{
    public string ExecuteTweak(string action)
    {
        return action switch
        {
            "Nagle" => SetRegistry(@"SYSTEM\CurrentControlSet\Services\Tcpip\Parameters\Interfaces", "TcpNoDelay", 1),
            "Nagle_Revert" => SetRegistry(@"SYSTEM\CurrentControlSet\Services\Tcpip\Parameters\Interfaces", "TcpNoDelay", 0),
            "HAGS" => SetRegistry(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwSchMode", 2),
            "HAGS_Revert" => SetRegistry(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwSchMode", 1),
            _ => "Unknown action"
        };
    }

    private string SetRegistry(string path, string valueName, int value)
    {
        try
        {
            using RegistryKey key = Registry.LocalMachine.CreateSubKey(path);
            key.SetValue(valueName, value, RegistryValueKind.DWord);
            return $"Set {valueName} to {value} at {path}";
        }
        catch (Exception ex) { return $"Error: {ex.Message}"; }
    }
}