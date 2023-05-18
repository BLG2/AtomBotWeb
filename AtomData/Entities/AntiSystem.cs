using AtomData.Entities;
using AtomData.Enums;
using MongoDB.Bson;

namespace AtomWeb.Models
{
    public class AntiSystem
    {
        public string _id { get; set; } = null!;
        public string GuildId { get; set; } = null!;
        public string LogChannelId { get; set; } = null!;
        public bool AntiLink { get; set; }
        public bool AntiMalLink { get; set; }
        public bool AntiSelfbot { get; set; }
        public bool AntiSpam { get; set; }
        public bool AntiMassJoin { get; set; }
        public bool AntiIp { get; set; }
        public bool AntiDeleteChannels { get; set; }
        public bool AntiGhostPing { get; set; }
        public bool AntiBots { get; set; }
        public bool AntiNonAscii { get; set; }
        public string Punishement { get; set; } = null!;
        public int MaxWarnings { get; set; }
        public List<string> ExcludeChannelIds { get; set; } = new List<string>();
        public List<string> ExcludeRoleIds { get; set; } = new List<string>();
        public List<string> ExcludeCategoryIds { get; set; } = new List<string>();

    }
}
