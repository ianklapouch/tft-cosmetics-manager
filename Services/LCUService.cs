using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace tft_cosmetics_manager.Services
{
    public class LCUService  
    {
        public static bool FetchKeys()
        {
            string command = "wmic";
            string arguments = "PROCESS WHERE name='LeagueClientUx.exe' GET commandline";

            Process process = new();

            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = $"/C{command} {arguments}";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;

            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            string appPortPattern = "--app-port=([0-9]*)";
            string authTokenPattern = "--remoting-auth-token=([\\w-]*)";

            Match appPortMatch = Regex.Match(output, appPortPattern);
            Match authTokenMatch = Regex.Match(output, authTokenPattern);

            if (appPortMatch.Success && authTokenMatch.Success)
            {
                App.BaseUrl = $"https://127.0.0.1:{appPortMatch.Groups[1].Value}";
                App.Auth = Convert.ToBase64String(Encoding.ASCII.GetBytes($"riot:{authTokenMatch.Groups[1].Value}"));
                return true;
            }
            return false;
        }
    }
}