using AtomData.Entities;
using AtomData.Models;

namespace AtomWeb.Models
{
    public class HomePageVM
    {
        public List<BotCommand>  BotCommands { get; set; } = new List<BotCommand>();
        public BotStatsModel BotStats { get; set; } = new BotStatsModel();
    }
}
