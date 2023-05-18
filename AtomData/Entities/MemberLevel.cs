using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomData.Entities
{
    public class MemberLevel
    {
        public string _id { get; set; } = null!;
        public string GuildId { get; set; } = null!;
        public string MemberId { get; set; } = null!;
        public int Level { get; set; }
        public int Xp { get; set; }
        public int TotalXp { get; set; }
    }
}
