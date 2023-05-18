using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomData.Entities
{
    public class Invite
    {
        public string _id { get; set; } = null!;
        public string GuildId { get; set; } = null!;
        public string ChannelId { get; set; } = null!;
        public bool Enabled { get; set; }
    }
}
