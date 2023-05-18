using AtomData.Models;
using AtomData.Services;
using AtomServices;
using AtomWeb.Filters;
using AtomWeb.Models;
using AtomWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace AtomWeb.Controllers
{
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class DiscordUserController : Controller
    {
        private readonly DiscordBotApiServices _discordBotApiServices;
        public DiscordUserController(DiscordBotApiServices discordBotApiServices)
        {
            _discordBotApiServices = discordBotApiServices;
        }

        public async Task<IActionResult> Index()
        {
            var accesToken = CookieService.GetSignedInAccesToken(HttpContext);
            if (accesToken == null) throw new Exception("Cound not fetch Auth2 AccesToken pls re-signin");
            var discordUser = await DiscordAuth.GetUserWithAccesTokenAsync(accesToken?.access_token ?? "123");
            if (discordUser == null) throw new Exception("Cound not fetch DiscordUser with AccesToken pls re-signin");
            var allGuilds = await DiscordAuth.GetUserGuildsWithAccesTokenAsync(accesToken?.access_token ?? "123");
            if (allGuilds == null) throw new Exception("Cound not fetch Guilds with AccesToken pls re-signin");
            var filteredGuilds = allGuilds.Where(g => g.permissions >= 2147483647).ToList();
            var mutualGuilds = await _discordBotApiServices.GetMutualDiscordServersAsync(new DiscordApiPostMutualServersModel { User = discordUser, Guilds = filteredGuilds });

            ViewData["BreadCrumb"] = BreadCrumbsService.AddBreadCrumbAsync(this, "Me");
            return View(new SignedInUserModel
            {
                DiscordUser = discordUser,
                AllDiscordGuilds = filteredGuilds ?? new List<DiscordGuild>(),
                MutualDiscordGuilds = mutualGuilds ?? new List<DiscordGuild>()
            });
        }

        private async Task ValidateUserLogin()
        {
            Regex CheaterReggex = new(@"(?<=\?code=).*(?=$)");
            var codeRegexMatch = CheaterReggex.Match(Request.QueryString.Value);
            string? code = codeRegexMatch.Success ? codeRegexMatch.Value : null;
            var token = await DiscordAuth.GetAuthTokenAsync(code ?? "123");
            if (token == null) throw new Exception("Accestoken returned null.");
            var discordUser = await DiscordAuth.GetUserWithAccesTokenAsync(token?.access_token ?? "123");
            if (discordUser == null) throw new Exception("Cound not fetch DiscordUser with AccesToken pls re-signin");
            await _discordBotApiServices.SendLoginMessageAsync(discordUser);
            CookieService.SetSignedInAccesToken(HttpContext, token);
        }

        public async Task<IActionResult> DiscordSignIn()
        {
            await ValidateUserLogin();
            PopupMessageService.SetPupupMessage(this, new NotifyVM { NotifyMessage = "Succesfully logged in.", Type = NotifyTypeEnum.Info }, "Home");
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public IActionResult DiscordSignOut()
        {
            Response.Cookies.Delete("SignedInAs");
            PopupMessageService.SetPupupMessage(this, new NotifyVM { NotifyMessage = "Succesfully logged out.", Type = NotifyTypeEnum.Warning }, "Home");
            return RedirectToAction("Index", "Home");
        }


    }
}
