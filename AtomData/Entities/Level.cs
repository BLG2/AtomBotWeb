using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomData.Entities
{
    public class Level
    {
        public string _id { get; set; } = null!;
        public string GuildId { get; set; } = null!;
        public bool Enabled { get; set; }
        public string? LogChannelId { get; set; }
        public int XpPerMessage { get; set; }
    }
}
