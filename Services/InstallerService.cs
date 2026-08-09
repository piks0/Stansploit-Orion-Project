using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace StansploitOrionProject.Services;

public class InstallerService
{
    public async Task<string> InstallPackageAsync(string packageId)
    {
        return await Task.Run(() =>
        {
            try
            {
                Process p = new Process();
                p.StartInfo.FileName = "winget";
                p.StartInfo.Arguments = $"install --id {packageId} --silent --accept-package-agreements --accept-source-agreements";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                
                p.Start();
                string output = p.StandardOutput.ReadToEnd();
                string error = p.StandardError.ReadToEnd();
                p.WaitForExit();

                if (p.ExitCode == 0)
                    return $"Successfully installed {packageId}.";
                else
                    return $"Failed {packageId} (Code {p.ExitCode}): {error}";
            }
            catch (Exception ex)
            {
                return $"Error executing winget: {ex.Message}. Ensure winget is installed.";
            }
        });
    }
}