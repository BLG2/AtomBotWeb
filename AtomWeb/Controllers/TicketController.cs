using AtomConfiguration;
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
using System.Drawing;
using static MongoDB.Bson.Serialization.Serializers.SerializerHelper;

namespace AtomWeb.Controllers
{
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class TicketController : Controller
    {
        private readonly MongoDbApiServices _mongoDbServices;
        private readonly DiscordBotApiServices _discordBotApiServices;

        public TicketController(MongoDbApiServices mongoDbServices, DiscordBotApiServices discordBotApiServices)
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
                CollectionName = "Ticket",
                SearchKey = new { GuildId = guildId }
            });
            var model = JsonConvert.DeserializeObject<TicketVM>(antiApiRes);
            if (model == null) model = new TicketVM { GuildId = guildId };
            model.GuildInfo = guildInfo;

            ViewData["Notify"] = PopupMessageService.GetPupupMessage(this);
            ViewData["BreadCrumb"] = BreadCrumbsService.AddBreadCrumbAsync(this, "Ticket Settings");

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveForm(TicketVM vm)
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
                    CollectionName = "Ticket",
                    ModelObject = JsonConvert.DeserializeObject<Ticket>(JsonConvert.SerializeObject(vm)),
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



        public async Task<IActionResult> SendDiscordMessage(string guildId)
        {
            var accesToken = CookieService.GetSignedInAccesToken(HttpContext);
            if (accesToken == null) throw new Exception("Cound not fetch Auth2 AccesToken pls re-signin");
            var discordUser = await DiscordAuth.GetUserWithAccesTokenAsync(accesToken?.access_token ?? "123");
            if (discordUser == null) throw new Exception("Cound not fetch DiscordUser with AccesToken pls re-signin");
            var userGuildValidation = await _discordBotApiServices.ValidateUserGuildAsync(guildId, discordUser?.id ?? "000");
            if (userGuildValidation == null || userGuildValidation.success == false) throw new Exception(userGuildValidation != null && !string.IsNullOrEmpty(userGuildValidation.message) ? userGuildValidation.message : "Somthing went whrong validating user and guild.");
            var guildInfo = await _discordBotApiServices.GetGuildInfoAsync(guildId, discordUser?.id ?? "00");
            if (guildInfo == null) throw new Exception("Cound not fetch DiscordGuild.");
            await _discordBotApiServices.SendDiscordMessageAsync(guildId, AtomData.Enums.DiscordMessageApiTypeEnum.Ticket);
            return RedirectToAction(nameof(Index), new { guildId = guildId });
        }


        public async Task<IActionResult> OpenTickets(string guildId)
        {
            var accesToken = CookieService.GetSignedInAccesToken(HttpContext);
            if (accesToken == null) throw new Exception("Cound not fetch Auth2 AccesToken pls re-signin");
            var discordUser = await DiscordAuth.GetUserWithAccesTokenAsync(accesToken?.access_token ?? "123");
            if (discordUser == null) throw new Exception("Cound not fetch DiscordUser with AccesToken pls re-signin");
            var userGuildValidation = await _discordBotApiServices.ValidateUserGuildAsync(guildId, discordUser?.id ?? "000");
            if (userGuildValidation == null || userGuildValidation.success == false) throw new Exception(userGuildValidation != null && !string.IsNullOrEmpty(userGuildValidation.message) ? userGuildValidation.message : "Somthing went whrong validating user and guild.");
            var guildInfo = await _discordBotApiServices.GetGuildInfoAsync(guildId, discordUser?.id ?? "00");
            if (guildInfo == null) throw new Exception("Cound not fetch DiscordGuild.");

            List<OpenTicket> messages = new List<OpenTicket>();
            var dbApiRes = await _mongoDbServices.GetDbObjectAsync(new ApiGetModel
            {
                DbName = "AtomBot",
                CollectionName = "OpenTicket",
                SearchKey = new { GuildId = guildId },
                ExpectingArray = true,
            });
            var dbApiResModel = JsonConvert.DeserializeObject<List<OpenTicket>>(dbApiRes);
            if (dbApiResModel != null) messages = dbApiResModel;
            ViewData["BreadCrumb"] = BreadCrumbsService.AddBreadCrumbAsync(this, "Open Tickets");
            ViewData["Notify"] = PopupMessageService.GetPupupMessage(this);

            return View(new OpenTicketsVM { GuildInfo = guildInfo, Member = guildInfo.Members.Where(m => m.id == discordUser.id).FirstOrDefault(), TicketMessages = messages.Where(x => guildInfo.Channels.Any(y => y.id == x.ChannelId)).OrderBy(x => x.TicketId).ThenBy(x => x.TicketOpened).ToList() });
        }



        public async Task<IActionResult> TicketInfo(string guildId, string dbTicketId)
        {
            var accesToken = CookieService.GetSignedInAccesToken(HttpContext);
            if (accesToken == null) throw new Exception("Cound not fetch Auth2 AccesToken pls re-signin");
            var discordUser = await DiscordAuth.GetUserWithAccesTokenAsync(accesToken?.access_token ?? "123");
            if (discordUser == null) throw new Exception("Cound not fetch DiscordUser with AccesToken pls re-signin");
            var userGuildValidation = await _discordBotApiServices.ValidateUserGuildAsync(guildId, discordUser?.id ?? "000");
            if (userGuildValidation == null || userGuildValidation.success == false) throw new Exception(userGuildValidation != null && !string.IsNullOrEmpty(userGuildValidation.message) ? userGuildValidation.message : "Somthing went whrong validating user and guild.");
            var guildInfo = await _discordBotApiServices.GetGuildInfoAsync(guildId, discordUser?.id ?? "00");
            if (guildInfo == null) throw new Exception("Cound not fetch DiscordGuild.");

            var dbApiRes = await _mongoDbServices.GetDbObjectAsync(new ApiGetModel
            {
                DbName = "AtomBot",
                CollectionName = "OpenTicket",
                SearchKey = new { GuildId = guildId, _id = new ObjectId(dbTicketId) },
            });
            var dbApiResModel = JsonConvert.DeserializeObject<OpenTicket>(dbApiRes);

            var lastTicketMessageCompoment = dbApiResModel?.TicketMessages.Select(x => JsonConvert.DeserializeObject<TicketMessage>(x)).Where(x => x.User_Id == PrivateConfig.BotId && x.Components.Count > 0).LastOrDefault() ?? null;
            var disableReply = lastTicketMessageCompoment?.Components?.Where(x => x.components.Where(y => y.custom_id == $"delete_ticket_{guildId}" || y.custom_id == $"reopen_ticket_{guildId}").Any()).Any() ?? false;

            var model = new TicketInfoVM
            {
                GuildInfo = guildInfo,
                Member = guildInfo.Members.Where(m => m.id == discordUser.id).FirstOrDefault(),
                OpenTicket = dbApiResModel,
                TicketReplyModel = new NewTickeMessageVM { guildId = guildId, memberId = discordUser.id, ticketObjectId = dbTicketId, Disabled = disableReply },
                TicketMessageButtonAction = new TicketMessageButtonActionVM { guildId = guildId, memberId = discordUser.id, ticketObjectId = dbTicketId }
            };

            ViewData["Notify"] = PopupMessageService.GetPupupMessage(this);
            ViewData["BreadCrumb"] = BreadCrumbsService.AddBreadCrumbAsync(this, "Ticket Info");

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendTicketMessage(NewTickeMessageVM model)
        {
            var accesToken = CookieService.GetSignedInAccesToken(HttpContext);
            if (accesToken == null) throw new Exception("Cound not fetch Auth2 AccesToken pls re-signin");
            var discordUser = await DiscordAuth.GetUserWithAccesTokenAsync(accesToken?.access_token ?? "123");
            if (discordUser == null) throw new Exception("Cound not fetch DiscordUser with AccesToken pls re-signin");
            var userGuildValidation = await _discordBotApiServices.ValidateUserGuildAsync(model.guildId, discordUser?.id ?? "000");
            if (userGuildValidation == null || userGuildValidation.success == false) throw new Exception(userGuildValidation != null && !string.IsNullOrEmpty(userGuildValidation.message) ? userGuildValidation.message : "Somthing went whrong validating user and guild.");
            var guildInfo = await _discordBotApiServices.GetGuildInfoAsync(model.guildId, discordUser?.id ?? "00");
            if (guildInfo == null) throw new Exception("Cound not fetch DiscordGuild.");

            if (ModelState.IsValid)
            {
                var messageSendApiRes = await _discordBotApiServices.SendTicketMessageAsync(model.guildId, model.memberId, model.ticketObjectId, model.Message);

                NotifyVM notify = new NotifyVM { NotifyMessage = "Somthing went whrong saving to the database.", Type = NotifyTypeEnum.Danger };
                if (messageSendApiRes != null)
                {
                    if (messageSendApiRes.succes)
                    {
                        notify.NotifyMessage = "Succesfully send message.";
                        notify.Type = NotifyTypeEnum.Succes;
                    }
                    if (!string.IsNullOrEmpty(messageSendApiRes.message))
                    {
                        notify.NotifyMessage = messageSendApiRes.message;
                        notify.Type = NotifyTypeEnum.Danger;
                    }
                }
                PopupMessageService.SetPupupMessage(this, notify);
                await Task.Delay(1000);
                return RedirectToAction(nameof(TicketInfo), new { guildId = model.guildId, dbTicketId = model.ticketObjectId });
            }


            var dbApiRes = await _mongoDbServices.GetDbObjectAsync(new ApiGetModel
            {
                DbName = "AtomBot",
                CollectionName = "OpenTicket",
                SearchKey = new { GuildId = model.guildId, _id = new ObjectId(model.ticketObjectId) },
            });

            var dbApiResModel = JsonConvert.DeserializeObject<OpenTicket>(dbApiRes);
            var lastTicketMessageCompoment = dbApiResModel?.TicketMessages.Select(x => JsonConvert.DeserializeObject<TicketMessage>(x)).Where(x => x.User_Id == PrivateConfig.BotId && x.Components.Count > 0).LastOrDefault() ?? null;
            var disableReply = lastTicketMessageCompoment?.Components?.Where(x => x.components.Where(y => y.custom_id == $"delete_ticket_{model.guildId}" || y.custom_id == $"reopen_ticket_{model.guildId}").Any()).Any() ?? false;

            var newModel = new TicketInfoVM
            {
                GuildInfo = guildInfo,
                Member = guildInfo.Members.Where(m => m.id == discordUser.id).FirstOrDefault(),
                OpenTicket = dbApiResModel,
                TicketReplyModel = model,
                TicketMessageButtonAction = new TicketMessageButtonActionVM { guildId = model.guildId, memberId = discordUser.id, ticketObjectId = model.ticketObjectId }
            };

            return View(nameof(TicketInfo), newModel);

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CloseTicket(TicketMessageButtonActionVM model)
        {
            var accesToken = CookieService.GetSignedInAccesToken(HttpContext);
            if (accesToken == null) throw new Exception("Cound not fetch Auth2 AccesToken pls re-signin");
            var discordUser = await DiscordAuth.GetUserWithAccesTokenAsync(accesToken?.access_token ?? "123");
            if (discordUser == null) throw new Exception("Cound not fetch DiscordUser with AccesToken pls re-signin");
            var userGuildValidation = await _discordBotApiServices.ValidateUserGuildAsync(model.guildId, discordUser?.id ?? "000");
            if (userGuildValidation == null || userGuildValidation.success == false) throw new Exception(userGuildValidation != null && !string.IsNullOrEmpty(userGuildValidation.message) ? userGuildValidation.message : "Somthing went whrong validating user and guild.");
            var guildInfo = await _discordBotApiServices.GetGuildInfoAsync(model.guildId, discordUser?.id ?? "00");
            if (guildInfo == null) throw new Exception("Cound not fetch DiscordGuild.");

            if (ModelState.IsValid)
            {
                var messageSendApiRes = await _discordBotApiServices.CloseTicketAsync(model.guildId, model.memberId, model.ticketObjectId);
                NotifyVM notify = new NotifyVM { NotifyMessage = "Somthing went whrong saving to the database.", Type = NotifyTypeEnum.Danger };
                if (messageSendApiRes != null)
                {
                    if (messageSendApiRes.succes)
                    {
                        notify.NotifyMessage = "Succesfully closed ticket.";
                        notify.Type = NotifyTypeEnum.Succes;
                    }
                    if (!string.IsNullOrEmpty(messageSendApiRes.message))
                    {
                        notify.NotifyMessage = messageSendApiRes.message;
                        notify.Type = NotifyTypeEnum.Danger;
                    }
                }
                PopupMessageService.SetPupupMessage(this, notify);
            }

            await Task.Delay(1000);
            return RedirectToAction(nameof(TicketInfo), new { guildId = model.guildId, dbTicketId = model.ticketObjectId });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReopenTicket(TicketMessageButtonActionVM model)
        {
            var accesToken = CookieService.GetSignedInAccesToken(HttpContext);
            if (accesToken == null) throw new Exception("Cound not fetch Auth2 AccesToken pls re-signin");
            var discordUser = await DiscordAuth.GetUserWithAccesTokenAsync(accesToken?.access_token ?? "123");
            if (discordUser == null) throw new Exception("Cound not fetch DiscordUser with AccesToken pls re-signin");
            var userGuildValidation = await _discordBotApiServices.ValidateUserGuildAsync(model.guildId, discordUser?.id ?? "000");
            if (userGuildValidation == null || userGuildValidation.success == false) throw new Exception(userGuildValidation != null && !string.IsNullOrEmpty(userGuildValidation.message) ? userGuildValidation.message : "Somthing went whrong validating user and guild.");
            var guildInfo = await _discordBotApiServices.GetGuildInfoAsync(model.guildId, discordUser?.id ?? "00");
            if (guildInfo == null) throw new Exception("Cound not fetch DiscordGuild.");

            if (ModelState.IsValid)
            {
                var messageSendApiRes = await _discordBotApiServices.ReopenTicketAsync(model.guildId, model.memberId, model.ticketObjectId);
                NotifyVM notify = new NotifyVM { NotifyMessage = "Somthing went whrong saving to the database.", Type = NotifyTypeEnum.Danger };
                if (messageSendApiRes != null)
                {
                    if (messageSendApiRes.succes)
                    {
                        notify.NotifyMessage = "Succesfully reopened ticket.";
                        notify.Type = NotifyTypeEnum.Succes;
                    }
                    if (!string.IsNullOrEmpty(messageSendApiRes.message))
                    {
                        notify.NotifyMessage = messageSendApiRes.message;
                        notify.Type = NotifyTypeEnum.Danger;
                    }
                }
                PopupMessageService.SetPupupMessage(this, notify);
            }
            await Task.Delay(1000);

            return RedirectToAction(nameof(TicketInfo), new { guildId = model.guildId, dbTicketId = model.ticketObjectId });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTicket(TicketMessageButtonActionVM model)
        {
            var accesToken = CookieService.GetSignedInAccesToken(HttpContext);
            if (accesToken == null) throw new Exception("Cound not fetch Auth2 AccesToken pls re-signin");
            var discordUser = await DiscordAuth.GetUserWithAccesTokenAsync(accesToken?.access_token ?? "123");
            if (discordUser == null) throw new Exception("Cound not fetch DiscordUser with AccesToken pls re-signin");
            var userGuildValidation = await _discordBotApiServices.ValidateUserGuildAsync(model.guildId, discordUser?.id ?? "000");
            if (userGuildValidation == null || userGuildValidation.success == false) throw new Exception(userGuildValidation != null && !string.IsNullOrEmpty(userGuildValidation.message) ? userGuildValidation.message : "Somthing went whrong validating user and guild.");
            var guildInfo = await _discordBotApiServices.GetGuildInfoAsync(model.guildId, discordUser?.id ?? "00");
            if (guildInfo == null) throw new Exception("Cound not fetch DiscordGuild.");

            if (ModelState.IsValid)
            {
                var messageSendApiRes = await _discordBotApiServices.DeleteTicketAsync(model.guildId, model.memberId, model.ticketObjectId);
                NotifyVM notify = new NotifyVM { NotifyMessage = "Somthing went whrong saving to the database.", Type = NotifyTypeEnum.Danger };
                if (messageSendApiRes != null)
                {
                    if (messageSendApiRes.succes)
                    {
                        notify.NotifyMessage = "Succesfully deleted ticket.";
                        notify.Type = NotifyTypeEnum.Succes;
                    }
                    if (!string.IsNullOrEmpty(messageSendApiRes.message))
                    {
                        notify.NotifyMessage = messageSendApiRes.message;
                        notify.Type = NotifyTypeEnum.Danger;
                    }
                }
                PopupMessageService.SetPupupMessage(this, notify);
            }

            await Task.Delay(1000);
            return RedirectToAction(nameof(OpenTickets), new { guildId = model.guildId });
        }























    }
}
