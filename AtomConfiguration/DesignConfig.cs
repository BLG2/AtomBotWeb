using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomConfiguration
{
    public class DesignConfig
    {
        public static string BotLogo { get; } = "https://imgur.com/Y8nmhSN.gif";

        public static List<KeyValuePair<string, string>> HomePageImageSlider { get; } = new List<KeyValuePair<string, string>>
                {
                new KeyValuePair<string, string>("SelfRole System","https://i.imgur.com/KZqpXwS.png"),
                new KeyValuePair<string, string>("Anti System","https://i.imgur.com/kCFOBsj.png"),
                new KeyValuePair<string, string>("Verification System","https://i.imgur.com/RyF9oWw.png"),
                new KeyValuePair<string, string>("Ticket System","https://i.imgur.com/eBukeFC.png"),
                new KeyValuePair<string, string>("And mutch more",""),
                };
    }
}
