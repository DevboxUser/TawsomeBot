
namespace TawsomeBot;


public class Config
{
    public string DiscordToken { get; set; }
    public ulong DiscordChannelId { get; set; }
    public ulong GuildId { get; set; }
    public string AlertMessage { get; set; }
    public string NotrunningMsg { get; set; }
    public string RunningMsg { get; set; }
    public string Process2Watch { get; set; }
    public int SecUntilScan { get; set; }
    public ulong[] AccessRoles { get; set; }
    public string NoAccess { get; set; }
    public string StartScript { get; set; }

}