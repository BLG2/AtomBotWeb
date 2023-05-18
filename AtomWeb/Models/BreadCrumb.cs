using System.Reflection.Metadata;

namespace AtomWeb.Models
{
    public class BreadCrumb
    {
        public string? Name { get; set; }
        public string Uri { get; set; } = null!;
    }
}
