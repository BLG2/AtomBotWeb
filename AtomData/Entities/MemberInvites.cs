using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomData.Entities
{
    public class MemberInvites
    {
        public string _id { get; set; } = null!;
        public string GuildId { get; set; } = null!;
        public string MemberId { get; set; } = null!;
        public int Joins { get; set; }
        public int Leaves { get; set; }
        public int Fakes { get; set; }
        public int Stays { get; set; }
        public List<string> InvitedUserIds { get; set; } = new List<string>();
    }
}
