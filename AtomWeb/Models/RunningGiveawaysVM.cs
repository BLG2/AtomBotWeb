using AtomData.Entities;
using AtomData.Models;

namespace AtomWeb.Models
{
    public class RunningGiveawaysVM : BaseVM
    {
        public string GuildId { get; set; } = null!;
        public List<Giveaway> Giveaways { get; set; } = new List<Giveaway>();   
    }
}
