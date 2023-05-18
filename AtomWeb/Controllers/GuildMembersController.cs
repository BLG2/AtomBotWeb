using AtomData.Entities;
using AtomData.Models;
using AtomData.Services;
using AtomServices;
using AtomWeb.Filters;
using AtomWeb.Models;
using AtomWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Data;
using static MongoDB.Bson.Serialization.Serializers.SerializerHelper;

namespace AtomWeb.Controllers
{
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class GuildMembersController : Controller
    {
        private readonly MongoDbApiServices _mongoDbServices;
        private readonly DiscordBotApiServices _discordBotApiServices;

        public GuildMembersController(MongoDbApiServices mongoDbServices, DiscordBotApiServices discordBotApiServices)
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
            var guildInfo = await _discordBotApiServices.GetGuildInfoAsync(guildId, discordUser?.id ?? "00");
            if (guildInfo == null) throw new Exception("Cound not fetch DiscordGuild.");

            ViewData["BreadCrumb"] = BreadCrumbsService.AddBreadCrumbAsync(this, "GuildMembers");

            return View(guildInfo);
        }

        public async Task<IActionResult> GuildMemberInfo(string guildId, string memberId)
        {
            var accesToken = CookieService.GetSignedInAccesToken(HttpContext);
            if (accesToken == null) throw new Exception("Cound not fetch Auth2 AccesToken pls re-signin");
            var discordUser = await DiscordAuth.GetUserWithAccesTokenAsync(accesToken?.access_token ?? "123");
            if (discordUser == null) throw new Exception("Cound not fetch DiscordUser with AccesToken pls re-signin");
            var guildInfo = await _discordBotApiServices.GetGuildInfoAsync(guildId, discordUser?.id ?? "00");
            if (guildInfo == null) throw new Exception("Cound not fetch DiscordGuild.");


            var memberInfoModel = new GuildMemberVM
            {
                GuildInfo = guildInfo,
                Member = guildInfo.Members.Where(m => m.id == memberId).FirstOrDefault()
            };

            foreach (DiscordGuildRole role in memberInfoModel.GuildInfo.Roles) memberInfoModel.SelectItemGuildRoles.Add(new SelectListItem { Text = role.name, Value = role.id });
            foreach (DiscordGuildRole role in memberInfoModel.Member.roles) memberInfoModel.SelectItemMemberRoles.Add(new SelectListItem { Text = role.name, Value = role.id });

            var warnings = await _mongoDbServices.GetDbObjectAsync(new ApiGetModel
            {
                DbName = "AtomBot",
                CollectionName = "Warnings",
                SearchKey = new { GuildId = guildId, MemberId = memberId },
                ExpectingArray = true
            });
            var warnModel = JsonConvert.DeserializeObject<List<MemberWarning>>(warnings);
            if (warnModel != null) memberInfoModel.MemberWarnings = warnModel;

            var level = await _mongoDbServices.GetDbObjectAsync(new ApiGetModel
            {
                DbName = "AtomBot",
                CollectionName = "MemberLevel",
                SearchKey = new { GuildId = guildId, MemberId = memberId }
            });
            var levelModel = JsonConvert.DeserializeObject<MemberLevel>(level);
            if (levelModel != null) memberInfoModel.MemberLevel = levelModel;

            var invites = await _mongoDbServices.GetDbObjectAsync(new ApiGetModel
            {
                DbName = "AtomBot",
                CollectionName = "MemberInvites",
                SearchKey = new { GuildId = guildId, MemberId = memberId }
            });
            var inviteModel = JsonConvert.DeserializeObject<MemberInvites>(invites);
            if (inviteModel != null) memberInfoModel.MemberInvites = inviteModel;

            ViewData["Notify"] = PopupMessageService.GetPupupMessage(this);
            ViewData["BreadCrumb"] = BreadCrumbsService.AddBreadCrumbAsync(this, "GuildMember Info");

            return View(memberInfoModel);
        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveMemberRoles(GuildMemberVM vm)
        {
            var accesToken = CookieService.GetSignedInAccesToken(HttpContext);
            if (accesToken == null) throw new Exception("Cound not fetch Auth2 AccesToken pls re-signin");
            var discordUser = await DiscordAuth.GetUserWithAccesTokenAsync(accesToken?.access_token ?? "123");
            if (discordUser == null) throw new Exception("Cound not fetch DiscordUser with AccesToken pls re-signin");
            var guildInfo = await _discordBotApiServices.GetGuildInfoAsync(vm?.GuildInfo?.DiscordGuild?.id ?? "000", discordUser?.id ?? "00");
            if (guildInfo == null) throw new Exception("Cound not fetch DiscordGuild.");
            var userGuildValidation = await _discordBotApiServices.ValidateUserGuildAsync(vm?.GuildInfo?.DiscordGuild?.id ?? "000", discordUser?.id ?? "000");
            if (userGuildValidation == null || userGuildValidation.success == false) throw new Exception(userGuildValidation != null && !string.IsNullOrEmpty(userGuildValidation.message) ? userGuildValidation.message : "Somthing went whrong validating user and guild.");


            var apiModel = new SetMemberRolesApiSendModel { GuildId = guildInfo.DiscordGuild.id, MemberId = discordUser.id };
            var chosenRoleIds = Request.Form["roles"];

            if (chosenRoleIds != StringValues.Empty && chosenRoleIds.Count > 0 && vm?.Member != null)
                apiModel.Roles = guildInfo.Roles.Where(gr => chosenRoleIds.Where(mr => gr.id == mr).FirstOrDefault() != null).ToList();

            var setRoleReply = await _discordBotApiServices.SetMemberRolesAsync(apiModel);
            if (setRoleReply != null && setRoleReply.Succes == true)
            {
                NotifyVM notify = new NotifyVM { NotifyMessage = "Succesfully edited roles.", Type = NotifyTypeEnum.Succes };
                PopupMessageService.SetPupupMessage(this, notify);
            }
            else
            {
                NotifyVM notify = new NotifyVM { NotifyMessage = "Somthing went whrong saving to the database.", Type = NotifyTypeEnum.Danger };
                if (!string.IsNullOrEmpty(setRoleReply?.Message ?? null))
                {
                    notify.NotifyMessage = setRoleReply.Message;
                    notify.Type = NotifyTypeEnum.Danger;
                }
                PopupMessageService.SetPupupMessage(this, notify);
            }

            await Task.Delay(3000);
            return RedirectToAction(nameof(GuildMemberInfo), new { guildId = vm.GuildInfo.DiscordGuild.id, memberId = vm.Member.id });
        }







        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetWarnings(string guildId, string memberId, string warnType)
        {
            var accesToken = CookieService.GetSignedInAccesToken(HttpContext);
            if (accesToken == null) throw new Exception("Cound not fetch Auth2 AccesToken pls re-signin");
            var discordUser = await DiscordAuth.GetUserWithAccesTokenAsync(accesToken?.access_token ?? "123");
            if (discordUser == null) throw new Exception("Cound not fetch DiscordUser with AccesToken pls re-signin");
            var guildInfo = await _discordBotApiServices.GetGuildInfoAsync(guildId ?? "000", discordUser?.id ?? "00");
            if (guildInfo == null) throw new Exception("Cound not fetch DiscordGuild.");
            var userGuildValidation = await _discordBotApiServices.ValidateUserGuildAsync(guildId ?? "000", discordUser?.id ?? "000");
            if (userGuildValidation == null || userGuildValidation.success == false) throw new Exception(userGuildValidation != null && !string.IsNullOrEmpty(userGuildValidation.message) ? userGuildValidation.message : "Somthing went whrong validating user and guild.");

            if (ModelState.IsValid)
            {
                var warning = await _mongoDbServices.GetDbObjectAsync(new ApiGetModel
                {
                    DbName = "AtomBot",
                    CollectionName = "Warnings",
                    SearchKey = new { GuildId = guildId, MemberId = memberId, WarnType = warnType }
                });
                var model = JsonConvert.DeserializeObject<MemberWarning>(warning);

                if (model != null)
                {
                    model.WarnCount = 0;
                    var antiApiRes = await _mongoDbServices.UpdateDbObjectAsync(new ApiPostModel
                    {
                        DbName = "AtomBot",
                        CollectionName = "Warnings",
                        ModelObject = model,
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
                }
                else
                {
                    PopupMessageService.SetPupupMessage(this, new NotifyVM { NotifyMessage = "Somthing went whrong saving to the database.", Type = NotifyTypeEnum.Danger });
                }
            }
            return RedirectToAction(nameof(GuildMemberInfo), new { guildId = guildId, memberId = memberId });
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetMemberLevel(GuildMemberVM vm)
        {
            var accesToken = CookieService.GetSignedInAccesToken(HttpContext);
            if (accesToken == null) throw new Exception("Cound not fetch Auth2 AccesToken pls re-signin");
            var discordUser = await DiscordAuth.GetUserWithAccesTokenAsync(accesToken?.access_token ?? "123");
            if (discordUser == null) throw new Exception("Cound not fetch DiscordUser with AccesToken pls re-signin");
            var guildInfo = await _discordBotApiServices.GetGuildInfoAsync(vm?.GuildInfo?.DiscordGuild?.id ?? "000", discordUser?.id ?? "00");
            if (guildInfo == null) throw new Exception("Cound not fetch DiscordGuild.");
            var userGuildValidation = await _discordBotApiServices.ValidateUserGuildAsync(vm?.GuildInfo?.DiscordGuild?.id ?? "000", discordUser?.id ?? "000");
            if (userGuildValidation == null || userGuildValidation.success == false) throw new Exception(userGuildValidation != null && !string.IsNullOrEmpty(userGuildValidation.message) ? userGuildValidation.message : "Somthing went whrong validating user and guild.");

            var level = await _mongoDbServices.GetDbObjectAsync(new ApiGetModel
            {
                DbName = "AtomBot",
                CollectionName = "MemberLevel",
                SearchKey = new { GuildId = vm?.GuildInfo?.DiscordGuild?.id ?? "000", MemberId = discordUser?.id ?? "00" }
            });
            var model = JsonConvert.DeserializeObject<MemberLevel>(level);
            if (model == null) model = new MemberLevel { GuildId = vm?.GuildInfo?.DiscordGuild?.id ?? "000", MemberId = discordUser?.id ?? "00" };

            if (model != null)
            {
                model.Level = vm.MemberLevel.Level;
                model.Xp = vm.MemberLevel.Xp;
                model.TotalXp = vm.MemberLevel.TotalXp;
                var antiApiRes = await _mongoDbServices.UpdateDbObjectAsync(new ApiPostModel
                {
                    DbName = "AtomBot",
                    CollectionName = "MemberLevel",
                    ModelObject = model,
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
            }
            else
            {
                PopupMessageService.SetPupupMessage(this, new NotifyVM { NotifyMessage = "Somthing went whrong.", Type = NotifyTypeEnum.Danger });
            }
            return RedirectToAction(nameof(GuildMemberInfo), new { guildId = vm.GuildInfo.DiscordGuild.id, memberId = vm.Member.id });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetMemberInvites(GuildMemberVM vm)
        {
            var accesToken = CookieService.GetSignedInAccesToken(HttpContext);
            if (accesToken == null) throw new Exception("Cound not fetch Auth2 AccesToken pls re-signin");
            var discordUser = await DiscordAuth.GetUserWithAccesTokenAsync(accesToken?.access_token ?? "123");
            if (discordUser == null) throw new Exception("Cound not fetch DiscordUser with AccesToken pls re-signin");
            var guildInfo = await _discordBotApiServices.GetGuildInfoAsync(vm?.GuildInfo?.DiscordGuild?.id ?? "000", discordUser?.id ?? "00");
            if (guildInfo == null) throw new Exception("Cound not fetch DiscordGuild.");
            var userGuildValidation = await _discordBotApiServices.ValidateUserGuildAsync(vm?.GuildInfo?.DiscordGuild?.id ?? "000", discordUser?.id ?? "000");
            if (userGuildValidation == null || userGuildValidation.success == false) throw new Exception(userGuildValidation != null && !string.IsNullOrEmpty(userGuildValidation.message) ? userGuildValidation.message : "Somthing went whrong validating user and guild.");

            var invites = await _mongoDbServices.GetDbObjectAsync(new ApiGetModel
            {
                DbName = "AtomBot",
                CollectionName = "MemberInvites",
                SearchKey = new { GuildId = vm?.GuildInfo?.DiscordGuild?.id ?? "000", MemberId = discordUser?.id ?? "00" }
            });
            var model = JsonConvert.DeserializeObject<MemberInvites>(invites);
            if (model == null) model = new MemberInvites { GuildId = vm?.GuildInfo?.DiscordGuild?.id ?? "000", MemberId = discordUser?.id ?? "00" };

            if (model != null)
            {
                model.Joins = vm.MemberInvites.Joins;
                model.Leaves = vm.MemberInvites.Leaves;
                model.Fakes = vm.MemberInvites.Fakes;
                model.Stays = vm.MemberInvites.Stays;
                var antiApiRes = await _mongoDbServices.UpdateDbObjectAsync(new ApiPostModel
                {
                    DbName = "AtomBot",
                    CollectionName = "MemberInvites",
                    ModelObject = model,
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
            }
            else
            {
                PopupMessageService.SetPupupMessage(this, new NotifyVM { NotifyMessage = "Somthing went whrong.", Type = NotifyTypeEnum.Danger });
            }
            return RedirectToAction(nameof(GuildMemberInfo), new { guildId = vm.GuildInfo.DiscordGuild.id, memberId = vm.Member.id });
        }


    }
}
