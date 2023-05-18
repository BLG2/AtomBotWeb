using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomData.Models
{
    public class LeaveEmbedModel
    {
        public string? MessageContent { get; set; }
        public string? EmbedAuthor { get; set; } = "@memberTag left the server.";
        public string? EmbedAuthorImageUrl { get; set; } = "@memberAvatar";
        public string? EmbedTitle { get; set; }
        public string? EmbedTitleUrl { get; set; }
        public string? EmbedDescription { get; set; } = "Member joined: @memberJoined";
        public string? EmbedThumbnailImageUrl { get; set; }
        public string? EmbedFooter { get; set; } = "@guildName - MemberCount: @memberCount";
        public string? EmbedFooterImageUrl { get; set; } = "@guildIcon";
        public string? EmbedImageUrl { get; set; }
    }
}
