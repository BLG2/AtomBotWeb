using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomData.Models
{
    public class ApiPostModel
    {
        public string DbName { get; set; } = null!;
        public string CollectionName { get; set; } = null!;
        public object ModelObject { get; set; } = null!;
        public double Time { get; set; }
    }
}
