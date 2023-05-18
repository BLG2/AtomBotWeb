using AtomData.Entities;
using AtomData.Enums;
using AtomData.Interfaces;
using AtomData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomServices
{
    public class DiscordBotApiServices : IDiscordBotApiRepository
    {
        private readonly IDiscordBotApiRepository _discordBotApiRepo;
        public DiscordBotApiServices(IDiscordBotApiRepository discordBotApiRepo)
        {
            _discordBotApiRepo = discordBotApiRepo;
        }

        public async Task<NewTicketMessageApiReply> CloseTicketAsync(string guildId, string memberId, string dbTicketObjectId) => await _discordBotApiRepo.CloseTicketAsync(guildId, memberId, dbTicketObjectId);
        public async Task<NewTicketMessageApiReply> ReopenTicketAsync(string guildId, string memberId, string dbTicketObjectId) => await _discordBotApiRepo.ReopenTicketAsync(guildId, memberId, dbTicketObjectId);
        public async Task<NewTicketMessageApiReply> DeleteTicketAsync(string guildId, string memberId, string dbTicketObjectId) => await _discordBotApiRepo.DeleteTicketAsync(guildId, memberId, dbTicketObjectId);
        public async Task<NewTicketMessageApiReply> SendTicketMessageAsync(string guildId, string memberId, string dbTicketObjectId, string message) => await _discordBotApiRepo.SendTicketMessageAsync(guildId, memberId, dbTicketObjectId, message);
        public async Task<List<BotCommand>> GetAllBotCommandsAsync() => await _discordBotApiRepo.GetAllBotCommandsAsync();
        public async Task<BotStatsModel> GetBotStatsAsync() => await _discordBotApiRepo.GetBotStatsAsync();
        public async Task<GuildInfoModel> GetGuildInfoAsync(string guildId, string memberId) => await _discordBotApiRepo.GetGuildInfoAsync(guildId, memberId);
        public async Task<List<DiscordGuild>> GetMutualDiscordServersAsync(DiscordApiPostMutualServersModel postModel) => await _discordBotApiRepo.GetMutualDiscordServersAsync(postModel);
        public async Task SendDiscordMessageAsync(string guildId, DiscordMessageApiTypeEnum Type) => await _discordBotApiRepo.SendDiscordMessageAsync(guildId, Type);
        public async Task SendLoginMessageAsync(DiscordUser user) => await _discordBotApiRepo.SendLoginMessageAsync(user);
        public async Task<SetMemberRolesApiReplyModel> SetMemberRolesAsync(SetMemberRolesApiSendModel model) => await _discordBotApiRepo.SetMemberRolesAsync(model);
        public async Task<StartGiveawayApiReplyModel> StartGiveaway(Giveaway model) => await _discordBotApiRepo.StartGiveaway(model);
        public async Task<UserGuildValidationApiModel> ValidateUserGuildAsync(string guildId, string userId) => await _discordBotApiRepo.ValidateUserGuildAsync(guildId, userId);

    }
}
