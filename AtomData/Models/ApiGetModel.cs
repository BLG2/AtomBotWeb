using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace AtomData.Models
{
    public class ApiGetModel
    {
        public string DbName { get; set; } = null!;
        public string CollectionName { get; set; } = null!;
        public object SearchKey { get; set; } = null!;
        public bool ExpectingArray { get; set; } = false;

        public double Time { get; set; }
    }
}
