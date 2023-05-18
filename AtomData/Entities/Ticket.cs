using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomData.Entities
{
    public class Ticket
    {
        public string _id { get; set; } = null!;
        public string GuildId { get; set; } = null!;
        public string ChannelId { get; set; } = null!;
        public string CategoryId { get; set; } = null!;
        public string TicketMessageId { get; set; } = null!;
        public string TicketMessage { get; set; } = null!;
        public string TicketOpenedMessage { get; set; } = null!;
        public string LogChannelId { get; set; } = null!;
        public string QueueChannelId { get; set; } = null!;
        public bool Enabled { get; set; }
        public int TotalOpenedTickets { get; set; }
        public List<string> RolesToTagIds { get; set; } = new List<string>();
    }
}
