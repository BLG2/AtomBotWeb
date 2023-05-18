using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomData.Models
{
    public class SetMemberRolesApiSendModel
    {
        public string GuildId { get; set; } = null!;
        public string MemberId { get; set; } = null!;
        public List<DiscordGuildRole> Roles { get; set; } = new List<DiscordGuildRole>();
    }
}
