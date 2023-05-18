using AtomData.Models;
using AtomWeb.Enums;
using System.ComponentModel.DataAnnotations;

namespace AtomWeb.Models
{
    public class NewGiveawayVM : BaseVM
    {
        public string? _id { get; set; } = "";
        public string GuildId { get; set; } = null!;
        [Required]
        public string ChannelId { get; set; } = null!;
        public string? MessageId { get; set; } = "";
        public string? LogChannelId { get; set; } = "";
        public double? Time { get; set; }
        public string Prize { get; set; } = "Somthing special";
        [Range(1, 1000)]
        public int Winners { get; set; } = 1;
        public int RequiredInvites { get; set; }
        public int RequiredMessages { get; set; }
        public int RequiredLevel { get; set; }
        public string? Note { get; set; }
        public bool Ended { get; set; } = false;
        public List<string> ExcludeRoleIds { get; set; } = new List<string>();
        public List<string> Attendees { get; set; } = new List<string>();


        public int SelectTime { get; set; } = 0;
        public TimeFormatEnum TimeFormat { get; set; } = TimeFormatEnum.Days;
    }
}
