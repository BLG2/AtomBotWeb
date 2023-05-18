namespace AtomData.Models
{
    public class DiscordGuild
    {
        public string id { get; set; } = null!;
        public string name { get; set; } = null!;
        public string? icon { get; set; }
        public bool owner { get; set; }
        public long permissions { get; set; }
        public List<string> features { get; set; } = new List<string>();
        public string? permissions_new { get; set; }
    }

}
