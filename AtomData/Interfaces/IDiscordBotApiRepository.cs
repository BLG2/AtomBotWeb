using AtomData.Entities;
using AtomData.Enums;
using AtomData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomData.Interfaces
{
    public interface IDiscordBotApiRepository
    {
        Task<List<DiscordGuild>> GetMutualDiscordServersAsync(DiscordApiPostMutualServersModel postModel);
        Task<GuildInfoModel> GetGuildInfoAsync(string guildId, string memberId);
        Task<UserGuildValidationApiModel> ValidateUserGuildAsync(string guildId, string userId);
        Task SendDiscordMessageAsync(string guildId, DiscordMessageApiTypeEnum Type);
        Task<List<BotCommand>> GetAllBotCommandsAsync();
        Task<BotStatsModel> GetBotStatsAsync();
        Task SendLoginMessageAsync(DiscordUser user);
        Task<SetMemberRolesApiReplyModel> SetMemberRolesAsync(SetMemberRolesApiSendModel userRolesModel);
        Task<StartGiveawayApiReplyModel> StartGiveaway(Giveaway model);
        Task<NewTicketMessageApiReply> SendTicketMessageAsync(string guildId, string memberId, string dbTicketObjectId, string message);
        Task<NewTicketMessageApiReply> CloseTicketAsync(string guildId, string memberId, string dbTicketObjectId);
        Task<NewTicketMessageApiReply> ReopenTicketAsync(string guildId, string memberId, string dbTicketObjectId);
        Task<NewTicketMessageApiReply> DeleteTicketAsync(string guildId, string memberId, string dbTicketObjectId);
    }
}
