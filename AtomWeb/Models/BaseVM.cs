using AtomData.Models;

namespace AtomWeb.Models
{
    public class BaseVM
    {
        public GuildInfoModel? GuildInfo { get; set; }
        public DiscordGuildMember? Member { get; set; }
    }
}
