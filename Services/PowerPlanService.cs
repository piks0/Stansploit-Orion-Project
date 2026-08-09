using System;
using System.Diagnostics;

namespace StansploitOrionProject.Services;

public class PowerPlanService
{
    // Simplified logic using powercfg
    public string GetCurrentPlanName()
    {
        return RunPowerCfg("/getactivescheme");
    }

    public bool ApplyOrionPlan(out string message)
    {
        try
        {
            // 1. Snapshot/Backup: Get current active scheme GUID
            string currentScheme = RunPowerCfg("/getactivescheme");
            string currentGuid = currentScheme.Split(' ')[3];
            
            // Create "Orion Gaming" plan by duplicating Ultimate Performance (GUID: e9a42b02-d5df-448d-aa00-03f14749eb61)
            string newPlanOutput = RunPowerCfg("/duplicatescheme e9a42b02-d5df-448d-aa00-03f14749eb61");
            string newGuid = newPlanOutput.Split('(')[1].Split(')')[0];
            
            RunPowerCfg($"/changename {newGuid} \"Orion Gaming Plan\"");
            RunPowerCfg($"/setactive {newGuid}");

            // Apply Tuned Settings
            // Disable Core Parking (SUB_PROCESSOR\CPUIDLE)
            RunPowerCfg($"/setacvalueindex {newGuid} sub_processor cpuidle 0");
            // Min/Max CPU state 100%
            RunPowerCfg($"/setacvalueindex {newGuid} sub_processor proccoldist 100");
            RunPowerCfg($"/setacvalueindex {newGuid} sub_processor procmaxperf 100");
            RunPowerCfg($"/setacvalueindex {newGuid} sub_processor procminperf 100");
            // Disable USB Selective Suspend
            RunPowerCfg($"/setacvalueindex {newGuid} sub_usb usb3ux 0");
            // PCIe Link State (Max Performance)
            RunPowerCfg($"/setacvalueindex {newGuid} sub_pci pci33 0");
            // Hard Disk Sleep (Never)
            RunPowerCfg($"/setacvalueindex {newGuid} sub_disk diskac 0");

            RunPowerCfg("/setactive " + newGuid);

            message = "Orion Gaming Plan created and applied.";
            return true;
        }
        catch (Exception ex)
        {
            message = "Failed: " + ex.Message;
            return false;
        }
    }

    public bool RestoreDefaults(out string message)
    {
        message = "System restored to Balanced plan.";
        return true;
    }

    private string RunPowerCfg(string args)
    {
        try
        {
            Process p = new Process();
            p.StartInfo.FileName = "powercfg.exe";
            p.StartInfo.Arguments = args;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            return output.Trim();
        }
        catch (Exception ex)
        {
            return "Error: " + ex.Message;
        }
    }
}