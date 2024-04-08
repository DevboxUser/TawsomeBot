using Newtonsoft.Json;

namespace TawsomeBot;

public class Installer
{
    public void Install()
    {
        Console.WriteLine("Please enter your discord token:");
        var token = Console.ReadLine();
        Console.WriteLine("Please enter your GuildID");
        var gId = ulong.Parse(Console.ReadLine()!);
        Console.WriteLine("Please enter your Channel ID");
        var cId = ulong.Parse(Console.ReadLine()!);
        var config = new Config();
        config.DiscordToken = token!;
        config.GuildId = gId;
        config.DiscordChannelId = cId;
        config.AlertMessage = "<@&1083877284710203536><@&1157739577272959096><@&1034920153235857418><@&1043876467341271080> SquadJS is not running!";
        config.NotrunningMsg = "SquadJs is not running.";
        config.Process2Watch = "node";
        config.SecUntilScan = 1800;//30min
        config.AccessRoles = new ulong[] { 1083877284710203536/*Server Tech*/, 1157739577272959096/*Server Admin*/,1034920153235857418/*DIVCOM*/,1043876467341271080/*BATCOM*/ ,1226651271092764764/*TestServerRole*/}; //RoleID's
        config.NoAccess = "Im sorry you do not have the role required for this.";
        config.StartScript = @"C:\Users\Administrator\Desktop\Start SquadJS (4.0.1).bat";
        // Save the config object to a file
        using (StreamWriter file = File.CreateText("config.json"))
        {
            var serializer = new JsonSerializer();
            serializer.Serialize(file, config);
        }
        Environment.Exit(0);
        
        
    }
}