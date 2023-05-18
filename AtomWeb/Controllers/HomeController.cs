using AspNet.Security.OAuth.Discord;
using AtomData.Entities;
using AtomData.Models;
using AtomData.Services;
using AtomServices;
using AtomWeb.Filters;
using AtomWeb.Models;
using AtomWeb.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Core.WireProtocol.Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Numerics;
using System.Reflection.Metadata;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace AtomWeb.Controllers
{
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DiscordBotApiServices _discordBotServices;

        public HomeController(ILogger<HomeController> logger, DiscordBotApiServices discordBotServices)
        {
            _logger = logger;
            _discordBotServices = discordBotServices;
        }

        public async Task<IActionResult> Index()
        {
            var homeModel = new HomePageVM
            {
                BotCommands = await _discordBotServices.GetAllBotCommandsAsync(),
                BotStats = await _discordBotServices.GetBotStatsAsync()
            };

            ViewData["Notify"] = PopupMessageService.GetPupupMessage(this);
            ViewData["BreadCrumb"] = BreadCrumbsService.AddBreadCrumbAsync(this, "Home");

            return View(homeModel);
        }




        [HttpPost]
        public JsonResult ChangeTheme(string theme)
        {
            CookieService.SetTheme(HttpContext, theme);
            return Json(new { success = true });
        }



        public IActionResult Privacy()
        {
            ViewData["BreadCrumb"] = BreadCrumbsService.AddBreadCrumbAsync(this, "Home");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}