using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomData.Entities
{
    public class Giveaway
    {
        public string _id { get; set; } = null!;
        public string GuildId { get; set; } = null!;
        public string ChannelId { get; set; } = null!;
        public string MessageId { get; set; } = null!;
        public string LogChannelId { get; set; } = null!;
        public double Time { get; set; }
        public string Prize { get; set; } = null!;
        public int Winners { get; set; }
        public int RequiredInvites { get; set; }
        public int RequiredMessages { get; set; }
        public int RequiredLevel { get; set; }
        public string Note { get; set; } = null!;
        public bool Ended { get; set; }
        public List<string> ExcludeRoleIds { get; set; } = new List<string>();
        public List<string> Attendees { get; set; } = new List<string>();
    }
}
