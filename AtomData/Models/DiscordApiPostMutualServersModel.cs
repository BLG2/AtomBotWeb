using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomData.Models
{
    public class DiscordApiPostMutualServersModel
    {
        public DiscordUser User { get; set; } = null!;
        public List<DiscordGuild> Guilds { get; set; } = null!;
    }
}
