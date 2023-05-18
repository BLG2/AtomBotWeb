using AtomData.Models;

namespace AtomWeb.Models
{
    public class SignedInUserModel
    {
        public DiscordUser DiscordUser { get; set; } = null!;
        public List<DiscordGuild> AllDiscordGuilds { get; set; } = new List<DiscordGuild>();
        public List<DiscordGuild> MutualDiscordGuilds { get; set; } = new List<DiscordGuild>();
    }
}
