using AtomData.Entities;

namespace AtomWeb.Models
{
    public class OpenTicketsVM : BaseVM
    {
        public List<OpenTicket> TicketMessages { get; set; } = new List<OpenTicket>();
    }
}
