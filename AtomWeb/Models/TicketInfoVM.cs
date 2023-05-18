using AtomData.Entities;
using AtomData.Models;
using Microsoft.Build.Framework;

namespace AtomWeb.Models
{
    public class TicketInfoVM : BaseVM
    {
        public OpenTicket? OpenTicket { get; set; }
        public NewTickeMessageVM TicketReplyModel { get; set; } = null!;
        public TicketMessageButtonActionVM TicketMessageButtonAction { get; set; } = null!;
    }



    public class NewTickeMessageVM
    {
        [Required]
        public string Message { get; set; } = null!;
        public string guildId { get; set; } = null!;
        public string memberId { get; set; } = null!;
        public string ticketObjectId { get; set; } = null!;
        public bool Disabled { get; set; } = false;
    }

    public class TicketMessageButtonActionVM
    {
        public Component? button { get; set; }
        public string guildId { get; set; } = null!;
        public string memberId { get; set; } = null!;
        public string ticketObjectId { get; set; } = null!;
    }


}
