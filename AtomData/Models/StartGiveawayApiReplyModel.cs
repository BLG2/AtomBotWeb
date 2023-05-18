using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomData.Models
{
    public class StartGiveawayApiReplyModel
    {
        public bool succes { get; set; }
        public string? messageId { get; set; }
        public string? message { get; set; }
        public double? endDate { get; set; }
    }
}
