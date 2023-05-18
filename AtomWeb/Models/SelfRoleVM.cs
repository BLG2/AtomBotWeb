using AtomData.Entities;
using AtomData.Models;

namespace AtomWeb.Models
{
    public class SelfRoleVM : BaseVM
    {
        public string? _id { get; set; }
        public bool Enabled { get; set; }
        public string? GuildId { get; set; }
        public string? ChannelId { get; set; }
        public List<string> RoleIds { get; set; } = new List<string>();
    }
}
