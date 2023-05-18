using AtomData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomData.Entities
{
    public class OpenTicket
    {
        public string _id { get; set; } = null!;
        public string GuildId { get; set; } = null!;
        public int TicketId { get; set; }
        public string ChannelId { get; set; } = null!;
        public string TicketMemberId { get; set; } = null!;
        public double TicketOpened { get; set; }
        public List<string> TicketMessages { get; set; } = new List<string>();
    }

    public class TicketMessage
    {
        public List<DiscordMessageAttachmentModel> Attachments { get; set; } = new List<DiscordMessageAttachmentModel>();
        public string Avatar { get; set; } = null!;
        public bool Bot { get; set; }
        public List<DiscordMessageComponentModel> Components { get; set; } = new List<DiscordMessageComponentModel>();
        public string Content { get; set; } = null!;
        public double? edited { get; set; }
        public double created { get; set; }
        public List<DiscordEmbedModel> Embeds { get; set; } = new List<DiscordEmbedModel>();
        public string Id { get; set; } = null!;
        public string Nick { get; set; } = null!;
        public string Tag { get; set; } = null!;
        public string User_Id { get; set; } = null!;
        public string Username { get; set; } = null!;
        public bool Verified { get; set; }
    }
}
