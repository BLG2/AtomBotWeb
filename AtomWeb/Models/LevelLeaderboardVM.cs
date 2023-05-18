using AtomData.Entities;
using AtomData.Models;

namespace AtomWeb.Models
{
    public class LevelLeaderboardVM : BaseVM
    {
        public string GuildId { get; set; } = null!;
        public List<MemberLevel> MemberLevels { get; set; } = new List<MemberLevel>();
    }
}
