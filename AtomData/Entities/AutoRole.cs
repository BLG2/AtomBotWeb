using AtomData.Entities;

namespace AtomWeb.Models
{
    public class AutoRole
    {
        public string _id { get; set; } = null!;
        public bool Enabled { get; set; }
        public string GuildId { get; set; } = null!;
        public List<string> RoleIds { get; set; } = new List<string>();
    }
}
