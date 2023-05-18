using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomData.Models
{
    public class BotStatsModel
    {
        public int ServerCount { get; set; }
        public int ChannelCount { get; set; }
        public int CommandCount { get; set; }
        public int MemberCount { get; set; }
        public int ShardCount { get; set; }
        public string DiscordVersion { get; set; } = "v14";
        public string NodeVersion { get; set; } = "v14";
        public long Uptime { get; set; }
    }
}
