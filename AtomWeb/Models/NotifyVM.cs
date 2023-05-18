namespace AtomWeb.Models
{
    public class NotifyVM
    {
        public NotifyTypeEnum Type { get; set; } = NotifyTypeEnum.Info;
        public string? NotifyMessage { get; set; }
    }

    public enum NotifyTypeEnum
    {
        Primary, Secondary, Succes, Danger, Warning, Info, Light, Dark
    }
}
