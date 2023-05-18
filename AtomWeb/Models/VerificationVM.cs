using AtomData.Entities;
using AtomData.Models;

namespace AtomWeb.Models
{
    public class VerificationVM : BaseVM
    {
        public string? _id { get; set; }
        public string? GuildId { get; set; }
        public string VerifiedRoleId { get; set; } = null!;
        public string? UnVerifiedRoleId { get; set; }
        public string ChannelId { get; set; } = null!;
        public string VerifyMessage { get; set; } = "Verify by clicking the ✅ below!";
        public string? MessageId { get; set; }
        public string VerifiedMessage { get; set; } = "Succesfully verifyed in @guildUrl";
        public bool Enabled { get; set; }
    }
}
