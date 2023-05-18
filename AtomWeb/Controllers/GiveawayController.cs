using AtomData.Entities;
using AtomData.Models;
using AtomData.Services;
using AtomServices;
using AtomWeb.Filters;
using AtomWeb.Models;
using AtomWeb.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json;
using static MongoDB.Bson.Serialization.Serializers.SerializerHelper;

namespace AtomWeb.Controllers
{
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class GiveawayController : Controller
    {
        private readonly MongoDbApiServices _mongoDbServices;
        private readonly DiscordBotApiServices _discordBotApiServices;
        public GiveawayController(MongoDbApiServices mongoDbServices, DiscordBotApiServices discordBotApiServices)
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

            var model = new RunningGiveawaysVM { GuildId = guildId, GuildInfo = guildInfo };

            var giveaways = await _mongoDbServices.GetDbObjectAsync(new ApiGetModel
            {
                DbName = "AtomBot",
                CollectionName = "Giveaway",
                SearchKey = new { GuildId = guildId },
                ExpectingArray = true
            });
            if (giveaways != null)
            {
                var allGiveaways = JsonConvert.DeserializeObject<List<Giveaway>>(giveaways);
                var activeGiveaways = allGiveaways?.Where(g => g.Ended == false).ToList() ?? new List<Giveaway>();
                model.Giveaways = activeGiveaways;
            }

            ViewData["Notify"] = PopupMessageService.GetPupupMessage(this);
            ViewData["BreadCrumb"] = BreadCrumbsService.AddBreadCrumbAsync(this, "Giveaway Settings");

            return View(model);
        }


        public async Task<IActionResult> NewGiveaway(string guildId, string? giveawayMessageId = null)
        {
            var accesToken = CookieService.GetSignedInAccesToken(HttpContext);
            if (accesToken == null) throw new Exception("Cound not fetch Auth2 AccesToken pls re-signin");
            var discordUser = await DiscordAuth.GetUserWithAccesTokenAsync(accesToken?.access_token ?? "123");
            if (discordUser == null) throw new Exception("Cound not fetch DiscordUser with AccesToken pls re-signin");
            var userGuildValidation = await _discordBotApiServices.ValidateUserGuildAsync(guildId, discordUser?.id ?? "000");
            if (userGuildValidation == null || userGuildValidation.success == false) throw new Exception(userGuildValidation != null && !string.IsNullOrEmpty(userGuildValidation.message) ? userGuildValidation.message : "Somthing went whrong validating user and guild.");
            var guildInfo = await _discordBotApiServices.GetGuildInfoAsync(guildId, discordUser?.id ?? "00");
            if (guildInfo == null) throw new Exception("Cound not fetch DiscordGuild.");

            var model = new NewGiveawayVM();
            if (!string.IsNullOrEmpty(giveawayMessageId))
            {
                var giveaway = await _mongoDbServices.GetDbObjectAsync(new ApiGetModel
                {
                    DbName = "AtomBot",
                    CollectionName = "Giveaway",
                    SearchKey = new { GuildId = guildId, MessageId = giveawayMessageId }
                });
                if (!string.IsNullOrEmpty(giveaway))
                {
                    var fetchedGiveaway = JsonConvert.DeserializeObject<Giveaway>(giveaway);
                    if (fetchedGiveaway != null)
                    {
                        model = new NewGiveawayVM
                        {
                            _id = fetchedGiveaway._id,
                            GuildId = fetchedGiveaway.GuildId,
                            ChannelId = fetchedGiveaway.ChannelId,
                            MessageId = giveawayMessageId,
                            LogChannelId = fetchedGiveaway.LogChannelId,
                            Time = fetchedGiveaway.Time,
                            Prize = fetchedGiveaway.Prize,
                            Winners = fetchedGiveaway.Winners,
                            RequiredInvites = fetchedGiveaway.RequiredInvites,
                            RequiredLevel = fetchedGiveaway.RequiredLevel,
                            RequiredMessages = fetchedGiveaway.RequiredMessages,
                            Note = fetchedGiveaway.Note,
                            Ended = fetchedGiveaway.Ended,
                            ExcludeRoleIds = fetchedGiveaway.ExcludeRoleIds,
                            Attendees = fetchedGiveaway.Attendees
                        };
                    }
                }
            }
            model.GuildInfo = guildInfo;
            model.GuildId = guildId;

            ViewData["Notify"] = PopupMessageService.GetPupupMessage(this);
            ViewData["BreadCrumb"] = BreadCrumbsService.AddBreadCrumbAsync(this, "New Giveaway");

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveForm(NewGiveawayVM vm)
        {
            var accesToken = CookieService.GetSignedInAccesToken(HttpContext);
            if (accesToken == null) throw new Exception("Cound not fetch Auth2 AccesToken pls re-signin");
            var discordUser = await DiscordAuth.GetUserWithAccesTokenAsync(accesToken?.access_token ?? "123");
            if (discordUser == null) throw new Exception("Cound not fetch DiscordUser with AccesToken pls re-signin");
            var userGuildValidation = await _discordBotApiServices.ValidateUserGuildAsync(vm?.GuildId ?? "000", discordUser?.id ?? "000");
            if (userGuildValidation == null || userGuildValidation.success == false) throw new Exception(userGuildValidation != null && !string.IsNullOrEmpty(userGuildValidation.message) ? userGuildValidation.message : "Somthing went whrong validating user and guild.");
            var guildInfo = await _discordBotApiServices.GetGuildInfoAsync(vm?.GuildId ?? "000", discordUser?.id ?? "000");
            if (guildInfo == null) throw new Exception("Cound not fetch DiscordGuild.");

            if (vm.SelectTime == 0 && vm.Time == null)
            {
                ModelState.AddModelError("Error", "SelectTime must be above 0");
            }
            if (ModelState.IsValid)
            {
                TimeSpan giveawayTime = new TimeSpan(3, 0, 0, 0);
                switch (vm.TimeFormat)
                {
                    case Enums.TimeFormatEnum.Months:
                        int inDays = vm.SelectTime * 30;
                        giveawayTime = new TimeSpan(inDays, 0, 0, 0);
                        break;
                    case Enums.TimeFormatEnum.Days:
                        giveawayTime = new TimeSpan(vm.SelectTime, 0, 0, 0);
                        break;
                    case Enums.TimeFormatEnum.Hours:
                        giveawayTime = new TimeSpan(0, vm.SelectTime, 0, 0);
                        break;
                    case Enums.TimeFormatEnum.Minutes:
                        giveawayTime = new TimeSpan(0, 0, vm.SelectTime, 0);
                        break;
                }

                Giveaway newGiveaway = new Giveaway
                {
                    _id = vm._id,
                    GuildId = vm.GuildId,
                    ChannelId = vm.ChannelId,
                    MessageId = vm.MessageId,
                    LogChannelId = vm.LogChannelId,
                    Prize = vm.Prize,
                    Winners = vm.Winners,
                    RequiredInvites = vm.RequiredInvites,
                    RequiredLevel = vm.RequiredLevel,
                    RequiredMessages = vm.RequiredMessages,
                    Note = vm.Note,
                    ExcludeRoleIds = vm.ExcludeRoleIds,
                    Attendees = vm.Attendees,
                    Time = vm.SelectTime != 0 ? DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + giveawayTime.TotalMilliseconds : (double)vm.Time
                };
                if (string.IsNullOrEmpty(newGiveaway.Note)) newGiveaway.Note = $"Hosted by: {(discordUser != null ? $"{discordUser.username}#{discordUser.discriminator}" : "Unknown")}";
                var sendedGiveawayMsg = await _discordBotApiServices.StartGiveaway(newGiveaway);

                if (sendedGiveawayMsg != null && sendedGiveawayMsg.succes == true && !string.IsNullOrEmpty(sendedGiveawayMsg.messageId))
                {
                    newGiveaway.MessageId = sendedGiveawayMsg.messageId;
                    var giveawayApiRes = await _mongoDbServices.UpdateDbObjectAsync(new ApiPostModel
                    {
                        DbName = "AtomBot",
                        CollectionName = "Giveaway",
                        ModelObject = newGiveaway,
                    });

                    if (giveawayApiRes == null) throw new Exception("Api responded with null.");

                    NotifyVM notify = new NotifyVM { NotifyMessage = "Somthing went whrong saving to the database.", Type = NotifyTypeEnum.Danger };
                    if (string.IsNullOrEmpty(giveawayApiRes.message) && (!string.IsNullOrEmpty(giveawayApiRes.insertedId) || giveawayApiRes.modifiedCount != 0))
                    {
                        notify.NotifyMessage = "Succesfully saved settings.";
                        notify.Type = NotifyTypeEnum.Succes;
                    }
                    if (!string.IsNullOrEmpty(giveawayApiRes.message))
                    {
                        notify.NotifyMessage = giveawayApiRes.message;
                        notify.Type = NotifyTypeEnum.Danger;
                    }
                    PopupMessageService.SetPupupMessage(this, notify);
                }
                else
                {
                    NotifyVM notify = new NotifyVM { NotifyMessage = "Somthing whrong sending the discord message.", Type = NotifyTypeEnum.Danger };
                    if (!string.IsNullOrEmpty(sendedGiveawayMsg.message))
                    {
                        notify.NotifyMessage = sendedGiveawayMsg.message;
                        notify.Type = NotifyTypeEnum.Danger;
                    }
                    PopupMessageService.SetPupupMessage(this, notify);
                }


                return RedirectToAction(nameof(Index), new { guildId = vm?.GuildId });
            }
            vm.GuildInfo = guildInfo;
            return View(nameof(NewGiveaway), vm);
        }




        public async Task<IActionResult> GiveawayInfo(string guildId, string giveawayMessageId)
        {
            var accesToken = CookieService.GetSignedInAccesToken(HttpContext);
            if (accesToken == null) throw new Exception("Cound not fetch Auth2 AccesToken pls re-signin");
            var discordUser = await DiscordAuth.GetUserWithAccesTokenAsync(accesToken?.access_token ?? "123");
            if (discordUser == null) throw new Exception("Cound not fetch DiscordUser with AccesToken pls re-signin");
            var userGuildValidation = await _discordBotApiServices.ValidateUserGuildAsync(guildId, discordUser?.id ?? "000");
            if (userGuildValidation == null || userGuildValidation.success == false) throw new Exception(userGuildValidation != null && !string.IsNullOrEmpty(userGuildValidation.message) ? userGuildValidation.message : "Somthing went whrong validating user and guild.");
            var guildInfo = await _discordBotApiServices.GetGuildInfoAsync(guildId, discordUser?.id ?? "00");
            if (guildInfo == null) throw new Exception("Cound not fetch DiscordGuild.");

            ViewData["Notify"] = PopupMessageService.GetPupupMessage(this);

            var giveaway = await _mongoDbServices.GetDbObjectAsync(new ApiGetModel
            {
                DbName = "AtomBot",
                CollectionName = "Giveaway",
                SearchKey = new { GuildId = guildId, MessageId = giveawayMessageId }
            });
            if (!string.IsNullOrEmpty(giveaway))
            {
                var fetchedGiveaway = JsonConvert.DeserializeObject<Giveaway>(giveaway);
                if (fetchedGiveaway != null)
                {
                    var model = new NewGiveawayVM
                    {
                        _id = fetchedGiveaway._id,
                        GuildId = fetchedGiveaway.GuildId,
                        ChannelId = fetchedGiveaway.ChannelId,
                        MessageId = giveawayMessageId,
                        LogChannelId = fetchedGiveaway.LogChannelId,
                        Time = fetchedGiveaway.Time,
                        Prize = fetchedGiveaway.Prize,
                        Winners = fetchedGiveaway.Winners,
                        RequiredInvites = fetchedGiveaway.RequiredInvites,
                        RequiredLevel = fetchedGiveaway.RequiredLevel,
                        RequiredMessages = fetchedGiveaway.RequiredMessages,
                        Note = fetchedGiveaway.Note,
                        Ended = fetchedGiveaway.Ended,
                        ExcludeRoleIds = fetchedGiveaway.ExcludeRoleIds,
                        Attendees = fetchedGiveaway.Attendees
                    };
                    model.GuildInfo = guildInfo;
                    model.GuildId = guildId;
                    return View(model);
                }
            }

            ViewData["BreadCrumb"] = BreadCrumbsService.AddBreadCrumbAsync(this, "Edit Giveaway");
            return RedirectToAction(nameof(Index), new { guildId = guildId });

        }



        /* [HttpPost]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> RemoveAttendeeFromGiveaway(string guildId, string giveawayMessageId, string memberId)
         {
             var giveaway = await _mongoDbServices.GetDbObjectAsync(new ApiGetModel
             {
                 DbName = "AtomBot",
                 CollectionName = "Giveaway",
                 SearchKey = new { GuildId = guildId, MessageId = giveawayMessageId }
             });

             var fetchedGiveaway = JsonConvert.DeserializeObject<Giveaway>(giveaway);
             fetchedGiveaway.Attendees.Remove(memberId);

             var giveawayApiRes = await _mongoDbServices.UpdateDbObjectAsync(new ApiPostModel
             {
                 DbName = "AtomBot",
                 CollectionName = "Giveaway",
                 ModelObject = fetchedGiveaway,
             });
             if (giveawayApiRes == null) throw new Exception("Api responded with null.");
             string notif = "Somthing went whrong saving to the database.";
             if (string.IsNullOrEmpty(giveawayApiRes.message) && (!string.IsNullOrEmpty(giveawayApiRes.insertedId) || giveawayApiRes.modifiedCount != 0)) notif = $"Succesfully removed attendee.";
             if (!string.IsNullOrEmpty(giveawayApiRes.message)) notif = giveawayApiRes.message;
             PopupMessageService.SetPupupMessage(this, notify);

             return RedirectToAction(nameof(GiveawayInfo), new { guildId = guildId, giveawayMessageId = giveawayMessageId });
         }*/











    }
}
