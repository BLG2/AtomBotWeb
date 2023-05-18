using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomData.Models
{
    public class DiscordMessageAttachmentModel
    {
        public string attachment { get; set; }
        public string name { get; set; }
        public string id { get; set; }
        public int size { get; set; }
        public string url { get; set; }
        public string proxyURL { get; set; }
        public object height { get; set; }
        public object width { get; set; }
        public string contentType { get; set; }
        public object description { get; set; }
        public bool ephemeral { get; set; }
    }
}
