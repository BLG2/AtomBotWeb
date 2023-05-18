using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomData.Entities
{
    public class MemberWarning
    {
        public string _id { get; set; } = null!;
        public string GuildId { get; set; } = null!;
        public string MemberId { get; set; } = null!;
        public string WarnType { get; set; } = null!;
        public int WarnCount { get; set; }
    }
}
