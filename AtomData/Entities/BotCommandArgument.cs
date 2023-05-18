using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomData.Entities
{
    public class BotCommandArgument
    {
        public string name { get; set; } = null!;
        public string description { get; set; } = null!;
        public bool required { get; set; }
        public List<string> options { get; set; } = new List<string>();
    }
}
