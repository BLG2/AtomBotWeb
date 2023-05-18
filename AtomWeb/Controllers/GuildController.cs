using AtomConfiguration;
using AtomData.Interfaces;
using AtomData.Models;
using AtomData.Services;
using AtomServices;
using AtomWeb.Filters;
using AtomWeb.Models;
using AtomWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace AtomWeb.Controllers
{
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class GuildController : Controller
    {
        private readonly DiscordBotApiServices _discordBotServices;
        public GuildController(DiscordBotApiServices discordBotServices)
        {
            _discordBotServices = discordBotServices;
        }

        public async Task<IActionResult> Index()
        {
            var accesToken = CookieService.GetSignedInAccesToken(HttpContext);
            if (accesToken == null) throw new Exception("Cound not fetch Auth2 AccesToken pls re-signin");
            var discordUser = await DiscordAuth.GetUserWithAccesTokenAsync(accesToken?.access_token ?? "123");
            if (discordUser == null) throw new Exception("Cound not fetch DiscordUser with AccesToken pls re-signin");
            var allGuilds = await DiscordAuth.GetUserGuildsWithAccesTokenAsync(accesToken?.access_token ?? "123");
            if (allGuilds == null) throw new Exception("Cound not fetch Guilds with AccesToken pls re-signin");
            var filteredGuilds = allGuilds;
            var mutualGuilds = await _discordBotServices.GetMutualDiscordServersAsync(new DiscordApiPostMutualServersModel { User = discordUser, Guilds = filteredGuilds });
            if (discordUser.id != PrivateConfig.BotOwnerId)
                filteredGuilds = allGuilds.Where(g => g.permissions >= 2147483647).ToList();

            ViewData["BreadCrumb"] = BreadCrumbsService.AddBreadCrumbAsync(this, "Guilds");

            return View(new SignedInUserModel
            {
                DiscordUser = discordUser,
                AllDiscordGuilds = filteredGuilds.OrderBy(x => mutualGuilds.Where(y => y.id == x.id).Any() == false).ThenBy(z => z.owner).ThenBy(a => a.name).ToList() ?? new List<DiscordGuild>(),
                MutualDiscordGuilds = mutualGuilds ?? new List<DiscordGuild>()
            });
        }


        public async Task<IActionResult> GuildInfo(string guildId)
        {
            var accesToken = CookieService.GetSignedInAccesToken(HttpContext);
            if (accesToken == null) throw new Exception("Cound not fetch Auth2 AccesToken pls re-signin");
            var discordUser = await DiscordAuth.GetUserWithAccesTokenAsync(accesToken?.access_token ?? "123");
            if (discordUser == null) throw new Exception("Cound not fetch DiscordUser with AccesToken pls re-signin");
            var guildInfo = await _discordBotServices.GetGuildInfoAsync(guildId, discordUser?.id ?? "00");
            if (guildInfo == null) throw new Exception("Cound not fetch DiscordGuild.");
            ViewData["BreadCrumb"] = BreadCrumbsService.AddBreadCrumbAsync(this, "Guild Bot Settings");
            return View(guildInfo);
        }


    }
}
