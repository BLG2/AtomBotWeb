using AtomData.Entities;
using AtomData.Models;
using System.ComponentModel.DataAnnotations;

namespace AtomWeb.Models
{
    public class LevelVM : BaseVM
    {
        public string? _id { get; set; }
        public string? GuildId { get; set; }
        public bool Enabled { get; set; }
        public string? LogChannelId { get; set; }

        [Range(1, 300)]
        public int XpPerMessage { get; set; }
    }
}
