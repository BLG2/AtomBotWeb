using AtomData.Entities;
using AtomData.Models;

namespace AtomWeb.Models
{
    public class ServerStatsVM : BaseVM
    {
        public string? _id { get; set; }
        public string? GuildId { get; set; }
        public bool Enabled { get; set; }
        public string? TotalMembersChannelId { get; set; }
        public string? MembersChannelId { get; set; }
        public string? BotsChannelId { get; set; }
        public string? OfflineChannelId { get; set; }
        public string? OnlineChannelId { get; set; }
        public string? IdleChannelId { get; set; }
        public string? DndChannelId { get; set; }
        public string? LiveChannelId { get; set; }
        public string? ChannelsChannelId { get; set; }
        public string? RolesChannelId { get; set; }
    }
}
