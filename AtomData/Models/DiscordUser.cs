namespace AtomData.Models
{
    public class DiscordUser
    {
        public string id { get; set; } = null!;
        public string username { get; set; } = null!;
        public string? avatar { get; set; }
        public string? avatar_decoration { get; set; }
        public string discriminator { get; set; } = null!;
        public string? public_flags { get; set; }
        public string? flags { get; set; }
        public string? banner { get; set; }
        public string? banner_color { get; set; }
        public string? accent_color { get; set; }
        public string? locale { get; set; }
        public string? mfa_enabled { get; set; }
        public string? premium_type { get; set; }


    }
}

