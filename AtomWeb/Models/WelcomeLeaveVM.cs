using AtomData.Entities;
using AtomData.Models;

namespace AtomWeb.Models
{
    public class WelcomeLeaveVM : BaseVM
    {
        public string? _id { get; set; }
        public string? GuildId { get; set; }
        public bool Enabled { get; set; }
        public string WelcomeChannelId { get; set; } = null!;
        public WelcomeEmbedModel WelcomeEmbed { get; set; } = new WelcomeEmbedModel();
        public string LeaveChannelId { get; set; } = null!;
        public LeaveEmbedModel LeaveEmbed { get; set; } = new LeaveEmbedModel();
    }
}
