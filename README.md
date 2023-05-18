# AtomBotWeb
WebPanel made for AtomBot made with love by BLG.

This wbsite is not fully ready yet and wil keep growing, also if you find any bug feel free to let me know or fix it yourselves.

[DiscordBot Code](https://github.com/BLG2/AtomBot)

[Live preview](https://atom-bot.xyz/)

----------

Make a new file in the AtomConfiguration folder called "PrivateConfig.cs", then add below content.
```
namespace AtomConfiguration
{
    public class PrivateConfig
    {
        public static string BotOwnerId { get; } = "YOUR_DISCORD_ID";
        public static string BotId { get; } = "DISCORD_BOT_ID";
        public static string client_secret { get; } = @"DISCORD_BOT_CLIENT_SECRET";
        public static string redirect_uri { get; } = "https://localhost:7134/DiscordUser/DiscordSignIn/";
        public static string XorKey { get; } = "XOR_KEY_SAME_AS_THE_BOT";
        public static string AesKey { get; } = "AES_KEY";
        public static string BotApiUrl { get; } = "http://localhost:8080";
        public static string DiscordInviteUrl { get; } = "DISCORD_INVITE_URL";
        public static string DiscordAuthUrl { get; } = "DISCORD_OAUTH_URL";
    }
}
```
