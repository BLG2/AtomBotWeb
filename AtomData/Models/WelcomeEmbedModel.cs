using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomData.Models
{
    public class WelcomeEmbedModel
    {
        public string? MessageContent { get; set; }
        public string? EmbedAuthor { get; set; } = "Hi, @memberTag";
        public string? EmbedAuthorImageUrl { get; set; } = "@memberAvatar";
        public string? EmbedTitle { get; set; }
        public string? EmbedTitleUrl { get; set; }
        public string? EmbedDescription { get; set; } = "Welcome to our server pls read our rules.";
        public string? EmbedThumbnailImageUrl { get; set; }
        public string? EmbedFooter { get; set; } = "@guildName - MemberCount: @memberCount";
        public string? EmbedFooterImageUrl { get; set; } = "@guildIcon";
        public string? EmbedImageUrl { get; set; }
    }
}
