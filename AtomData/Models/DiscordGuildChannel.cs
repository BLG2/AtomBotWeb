using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomData.Models
{
    public class DiscordGuildChannel
    {
        public string id { get; set; } = null!;
        public string name { get; set; } = null!;
        public int Type { get; set; }
    }
}
