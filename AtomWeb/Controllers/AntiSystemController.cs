﻿using AtomData.Entities;
using AtomData.Models;
using AtomData.Services;
using AtomServices;
using AtomWeb.Filters;
using AtomWeb.Models;
using AtomWeb.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;
using MongoDB.Bson;
using Newtonsoft.Json;
using System.Text;

namespace AtomWeb.Controllers
{
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class AntiSystemController : Controller
    {
        private readonly MongoDbApiServices _mongoDbServices;
        private readonly DiscordBotApiServices _discordBotApiServices;
        public AntiSystemController(MongoDbApiServices mongoDbServices, DiscordBotApiServices discordBotApiServices)
        {
            _mongoDbServices = mongoDbServices;
            _discordBotApiServices = discordBotApiServices;
        }

        public async Task<IActionResult> Index(string guildId)
        {
            var accesToken = CookieService.GetSignedInAccesToken(HttpContext);
            if (accesToken == null) throw new Exception("Cound not fetch Auth2 AccesToken pls re-signin");
            var discordUser = await DiscordAuth.GetUserWithAccesTokenAsync(accesToken?.access_token ?? "123");
            if (discordUser == null) throw new Exception("Cound not fetch DiscordUser with AccesToken pls re-signin");
            var userGuildValidation = await _discordBotApiServices.ValidateUserGuildAsync(guildId, discordUser?.id ?? "000");
            if (userGuildValidation == null || userGuildValidation.success == false) throw new Exception(userGuildValidation != null && !string.IsNullOrEmpty(userGuildValidation.message) ? userGuildValidation.message : "Somthing went whrong validating user and guild.");
            var guildInfo = await _discordBotApiServices.GetGuildInfoAsync(guildId, discordUser?.id ?? "00");
            if (guildInfo == null) throw new Exception("Cound not fetch DiscordGuild.");
            var antiApiRes = await _mongoDbServices.GetDbObjectAsync(new ApiGetModel
            {
                DbName = "AtomBot",
                CollectionName = "AntiSystem",
                SearchKey = new { GuildId = guildId }
            });
            var model = JsonConvert.DeserializeObject<AntiSystemVM>(antiApiRes);
            if (model == null) model = new AntiSystemVM { GuildId = guildId };
            model.GuildInfo = guildInfo;

            ViewData["Notify"] = PopupMessageService.GetPupupMessage(this);
            ViewData["BreadCrumb"] = BreadCrumbsService.AddBreadCrumbAsync(this, "AntySystem Settings");

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveForm(AntiSystemVM vm)
        {
            var accesToken = CookieService.GetSignedInAccesToken(HttpContext);
            if (accesToken == null) throw new Exception("Cound not fetch Auth2 AccesToken pls re-signin");
            var discordUser = await DiscordAuth.GetUserWithAccesTokenAsync(accesToken?.access_token ?? "123");
            if (discordUser == null) throw new Exception("Cound not fetch DiscordUser with AccesToken pls re-signin");
            var userGuildValidation = await _discordBotApiServices.ValidateUserGuildAsync(vm?.GuildId ?? "000", discordUser?.id ?? "000");
            if (userGuildValidation == null || userGuildValidation.success == false) throw new Exception(userGuildValidation != null && !string.IsNullOrEmpty(userGuildValidation.message) ? userGuildValidation.message : "Somthing went whrong validating user and guild.");

            if (ModelState.IsValid)
            {
                var antiApiRes = await _mongoDbServices.UpdateDbObjectAsync(new ApiPostModel
                {
                    DbName = "AtomBot",
                    CollectionName = "AntiSystem",
                    ModelObject = JsonConvert.DeserializeObject<AntiSystem>(JsonConvert.SerializeObject(vm)),
                });

                if (antiApiRes == null) throw new Exception("Api responded with null.");

                NotifyVM notify = new NotifyVM { NotifyMessage = "Somthing went whrong saving to the database.", Type = NotifyTypeEnum.Danger };
                if (string.IsNullOrEmpty(antiApiRes.message) && (!string.IsNullOrEmpty(antiApiRes.insertedId) || antiApiRes.modifiedCount != 0))
                {
                    notify.NotifyMessage = "Succesfully saved settings.";
                    notify.Type = NotifyTypeEnum.Succes;
                }
                if (!string.IsNullOrEmpty(antiApiRes.message))
                {
                    notify.NotifyMessage = antiApiRes.message;
                    notify.Type = NotifyTypeEnum.Danger;
                }
                PopupMessageService.SetPupupMessage(this, notify);


                return RedirectToAction(nameof(Index), new { guildId = vm?.GuildId });
            }
            return View(nameof(Index), vm);
        }



    }
}