using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomData.Models
{
    public class DbUpdateApiResponse
    {
        public bool acknowledged { get; set; }
        public int modifiedCount { get; set; }
        public int matchedCount { get; set; }
        public string? insertedId { get; set; }
        public string? message { get; set; }
    }
}
