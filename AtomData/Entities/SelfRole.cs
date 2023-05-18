using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomData.Entities
{
    public class SelfRole
    {
        public string _id { get; set; } = null!;
        public bool Enabled { get; set; }
        public string GuildId { get; set; } = null!;
        public string ChannelId { get; set; } = null!;
        public List<string> RoleIds { get; set; } = new List<string>();
    }
}
