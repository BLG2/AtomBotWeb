using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomData.Entities
{
    public class Verification
    {
        public string _id { get; set; } = null!;
        public string GuildId { get; set; } = null!;
        public string VerifiedRoleId { get; set; } = null!;
        public string UnVerifiedRoleId { get; set; } = null!;
        public string ChannelId { get; set; } = null!;
        public string VerifyMessage { get; set; } = null!;
        public string MessageId { get; set; } = null!;
        public string VerifiedMessage { get; set; } = null!;
        public bool Enabled { get; set; }
    }
}
