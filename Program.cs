//**
//**
//**
////

using Discord;
using Discord.Net;
using Discord.WebSocket;
using Newtonsoft.Json;
using TawsomeBot;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

public class Bot
{
    private static DiscordSocketClient _client;
    
    static Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());// Logs the string of the LogMessage to console.
        return Task.CompletedTask;
    }

    private static Config _config;
    public static async Task Main()
    {
        try
        {
            var configFile = Environment.CurrentDirectory + "\\config.json";
            if (File.Exists(configFile))
            {

                _config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(configFile))!;

                //Initialization of the _client
                _client = new DiscordSocketClient();
                _client.Log += Log;
                await _client.LoginAsync(TokenType.Bot, _config.DiscordToken);
                await _client.StartAsync();


                _client.Ready += Client_Ready;

                _client.SlashCommandExecuted += SlashCommandHandler;


            }
            else
            {
                Console.WriteLine("Config file not found! please run the installer.");
                var install = new Installer();
                install.Install();
            }

            await Task.Delay(-1);
        } catch (Exception ex)
        {
            Console.WriteLine($"msg: {ex.Message} source: {ex.Source}");
        }
    }

    public static async Task Client_Ready()
    {

         var guildCommand = new SlashCommandBuilder()
        .WithName("sqbot")
        .WithDescription("The Tawsome Squad Bot.")
        .AddOption(new SlashCommandOptionBuilder()
            .WithName("status")
            .WithDescription("Gets the status of SquadJs")
            .WithType(ApplicationCommandOptionType.SubCommand)
        ).AddOption(new SlashCommandOptionBuilder()
            .WithName("start")
            .WithDescription("starts the Squadjs server")
            .WithType(ApplicationCommandOptionType.SubCommand)
        ).AddOption(new SlashCommandOptionBuilder()
            .WithName("stop")
            .WithDescription("stops the Squadjs server")
            .WithType(ApplicationCommandOptionType.SubCommand)
        ).AddOption(new SlashCommandOptionBuilder()
            .WithName("restart")
            .WithDescription("Restarts the Squadjs sever")
            .WithType(ApplicationCommandOptionType.SubCommand)

        );

    try
    {
        await _client.Rest.CreateGuildCommand(guildCommand.Build(), _config.GuildId);
        await ProcessChecker.Monitor(_client, _config);
    }
    catch(ApplicationCommandException exception)
    {
        var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
        Console.WriteLine(json);
    }
}
    private static async Task HandleSqbotCommand(SocketSlashCommand command)
    {
        try
        {


            // First lets extract our variables
            var fieldName = command.Data.Options.First().Name;

            switch (fieldName)
            {
                case "start":
                {
                    await command.RespondAsync($"Server is starting!");
                    ProcessChecker.Start(_config);
                    

                }
                    break;
                case "stop":
                {
                    await command.RespondAsync($"Server is stopping!");
                    ProcessChecker.Stop();
                    
                }
                    break;
                case "restart":
                {
                    await command.RespondAsync($"Server is restarting!");
                    ProcessChecker.Restart(_config);
                   
                }
                    break;
                case "status":
                {
                    //await ProcessChecker.Start(30,_client,_config);
                    await ProcessChecker.Check(command, _config);

                }
                    break;
            }
        } catch (Exception ex)
        {
            Console.WriteLine($"msg: {ex.Message} source: {ex.Source}");
        }
    }

    private static bool isValid(SocketSlashCommand cmd)
    {
        var guild = _client.GetGuild(_config.GuildId);
        var user = guild.GetUser(cmd.User.Id);
        var roles = user.Roles.Select(r => r.Id);
        return roles.Intersect(_config.AccessRoles).Any();
    }
    private static async Task SlashCommandHandler(SocketSlashCommand command)
    {
        if (command.User.IsBot) return;
        if (isValid(command))
        {
            Console.WriteLine("Access Granted!");
            switch(command.Data.Name)
            {
                case "sqbot":
                    await HandleSqbotCommand(command);
                    break;
           
            }
        }
        else
        {
            Console.WriteLine("Access Denied!");
            await command.RespondAsync($"{_config.NoAccess}");
        }
       
    }
    
}