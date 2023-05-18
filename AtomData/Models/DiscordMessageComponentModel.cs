using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomData.Models
{
    public class Component
    {
        public int type { get; set; }
        public int style { get; set; }
        public string label { get; set; }
        public Emoji emoji { get; set; }
        public string custom_id { get; set; }
    }

    public class Emoji
    {
        public string name { get; set; }
    }

    public class DiscordMessageComponentModel
    {
        public int type { get; set; }
        public List<Component> components { get; set; }
    }
}
