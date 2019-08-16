using System;
using System.Collections.Generic;

namespace BenchWarmerAPI.Models
{
    public partial class Classes
    {
        public Classes()
        {
            Characters = new HashSet<Characters>();
        }

        public int ClassId { get; set; }
        public string ClassName { get; set; }

        public ICollection<Characters> Characters { get; set; }
    }
}
