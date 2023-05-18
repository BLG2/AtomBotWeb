using AtomData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomData.Entities
{
    public class WelcomeLeave
    {
        public string _id { get; set; } = null!;
        public string GuildId { get; set; } = null!;
        public bool Enabled { get; set; }
        public string WelcomeChannelId { get; set; } = null!;
        public WelcomeEmbedModel WelcomeEmbed { get; set; } = new WelcomeEmbedModel();
        public string LeaveChannelId { get; set; } = null!;
        public LeaveEmbedModel LeaveEmbed { get; set; } = new LeaveEmbedModel();
    }
}
