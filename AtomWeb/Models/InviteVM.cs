using AtomData.Entities;
using AtomData.Models;

namespace AtomWeb.Models
{
    public class InviteVM : BaseVM
    {
        public string? _id { get; set; }
        public string? GuildId { get; set; }
        public string? ChannelId { get; set; }
        public bool Enabled { get; set; }
    }
}
