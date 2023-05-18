using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomData.Entities
{
    public class BotCommand
    {
        public string CommandName { get; set; } = null!;
        public string CommandDescription { get; set; } = null!;
        public string Categorie { get; set; } = null!;
        public List<BotCommandArgument> Arguments { get; set; } = null!;
    }
}
