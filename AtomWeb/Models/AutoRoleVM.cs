using AtomData.Entities;
using AtomData.Models;

namespace AtomWeb.Models
{
    public class AutoRoleVM : BaseVM
    {
        public string? _id { get; set; }
        public bool Enabled { get; set; }
        public string GuildId { get; set; } = null!;
        public List<string> RoleIds { get; set; } = new List<string>();
    }
}
