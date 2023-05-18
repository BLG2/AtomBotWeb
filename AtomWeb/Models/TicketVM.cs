using AtomData.Models;

namespace AtomWeb.Models
{
    public class TicketVM : BaseVM
    {
        public string? _id { get; set; } 
        public string? GuildId { get; set; }
        public string? ChannelId { get; set; }
        public string? CategoryId { get; set; }
        public string? TicketMessageId { get; set; }
        public string TicketMessage { get; set; } = "React with 📨 to open an ticket.";
        public string TicketOpenedMessage { get; set; } = "Hi @member, drop your question here and support wil help you asap.";
        public string? LogChannelId { get; set; } 
        public string? QueueChannelId { get; set; }
        public bool Enabled { get; set; }
        public int TotalOpenedTickets { get; set; }
        public List<string> RolesToTagIds { get; set; } = new List<string>();
    }
}
