using AtomData.Entities;
using AtomData.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AtomWeb.Models
{
    public class GuildMemberVM : BaseVM
    {
        public List<MemberWarning> MemberWarnings { get; set; } = new List<MemberWarning>();
        public MemberLevel MemberLevel { get; set; } = new MemberLevel();
        public MemberInvites MemberInvites { get; set; } = new MemberInvites();

        public List<SelectListItem> SelectItemGuildRoles { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> SelectItemMemberRoles { get; set; } = new List<SelectListItem>();
    }
}
