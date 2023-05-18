using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomData.Models
{
    public class DiscordGuildMember
    {
        public string id { get; set; } = null!;
        public string name { get; set; } = null!;
        public string discriminator { get; set; } = null!;
        public string? avatar { get; set; }
        public string? permissions { get; set; } = null!;
        public List<DiscordGuildRole> roles { get; set; } = new List<DiscordGuildRole>();
    }
}
