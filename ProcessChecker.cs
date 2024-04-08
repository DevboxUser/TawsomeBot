using System.Data.SqlTypes;
using Discord.WebSocket;

namespace TawsomeBot;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

public class ProcessChecker
{
    private static bool isToRun;
    private static bool isRunning;
    private static Process _process;
    public static async Task Monitor(DiscordSocketClient _client,Config _c)
    {
        isToRun = true;
        while (isToRun)
        {
            if (!IsNodeRunning(_c))
            {
                 var chan = (SocketTextChannel)(_client.GetChannel(_c.DiscordChannelId)); 
                 await chan.SendMessageAsync($"{_c.AlertMessage.ToString()}");
            }

           

            await Task.Delay(TimeSpan.FromSeconds(_c.SecUntilScan));  // wait for 5 seconds before next check
        }
    }
    
    public static async Task Check(SocketSlashCommand command,Config _c) {
            if (!IsNodeRunning(_c))
            {
                await command.RespondAsync($"{_c.NotrunningMsg}");
            }
        
    }

    public static void Stop()
    {
        isToRun = false;
        isRunning = false;
        _process.Kill();
        
    }
    public static void Start(Config _c)
    {
        isRunning = true;
        var psi = new ProcessStartInfo();
        psi.FileName = _c.StartScript;
        psi.RedirectStandardOutput = false;
        psi.RedirectStandardError = false;
        psi.UseShellExecute = false;

        _process = new Process();
        _process.StartInfo = psi;
        _process.Start();

       _process.WaitForExit();
    }

    public static void Restart(Config _c)
    {
        Stop();
        Start(_c);
    }
    
    
    private static bool IsNodeRunning(Config _c)
    {
        return Process.GetProcesses().Any(p => p.ProcessName == _c.Process2Watch);
    }
}