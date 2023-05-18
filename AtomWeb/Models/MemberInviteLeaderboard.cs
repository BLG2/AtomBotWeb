using AtomData.Entities;
using AtomData.Models;

namespace AtomWeb.Models
{
    public class MemberInviteLeaderboard : BaseVM
    {
        public string GuildId { get; set; } = null!;
        public List<MemberInvites> MemberInvites { get; set; } = new List<MemberInvites>();
    }
}
