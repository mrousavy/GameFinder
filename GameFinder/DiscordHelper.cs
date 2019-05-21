using System;
using DiscordRPC;
using DiscordRPC.Logging;

namespace GameFinder
{
    public class DiscordHelper
    {
        private const string DiscordClientId = "580377534089134093";
        public static DiscordRpcClient Client { get; set; }

        public static void Initialize()
        {
            Client = new DiscordRpcClient(DiscordClientId) { Logger = new ConsoleLogger { Level = LogLevel.Warning } };

            Client.OnReady += (sender, e) => { Console.WriteLine($"Discord ready! {e.User.Username}"); };

            Client.OnPresenceUpdate += (sender, e) => { Console.WriteLine($"Presence updated: {e.Presence}"); };

            Client.Initialize();
        }

        public static void SetPresence(string status)
        {
            if (!Client.IsInitialized)
            {
                return;
            }

            Client.SetPresence(new RichPresence
            {
                Details = "GitHub: mrousavy/GameFinder",
                State = status,
                Assets = new Assets
                {
                    LargeImageKey = "icon",
                    LargeImageText = "Steam GameFinder by mrousavy"
                }
            });
        }
    }
}