using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomData.Models
{
    public class GuildInfoModel
    {
        public string GuildId { get; set; } = null!;
        public List<DiscordGuildChannel> Channels { get; set; } = new List<DiscordGuildChannel>();
        public List<DiscordGuildRole> Roles { get; set; } = new List<DiscordGuildRole>();
        public List<DiscordGuildMember> Members { get; set; } = new List<DiscordGuildMember>();
        public DiscordGuild? DiscordGuild { get; set; }
    }
}
