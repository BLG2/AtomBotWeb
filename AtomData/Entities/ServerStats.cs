using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomData.Entities
{
    public class ServerStats
    {
        public string _id { get; set; } = null!;
        public string GuildId { get; set; } = null!;
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
        public string? TotalMembers { get; set; }
    }
}
