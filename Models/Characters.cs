using System;
using System.Collections.Generic;

namespace BenchWarmerAPI.Models
{
    public partial class Characters
    {
        public int CharacterId { get; set; }
        public string FullName { get; set; }
        public int? UserIdFk { get; set; }
        public int? ClassIdFk { get; set; }

        public Classes ClassIdFkNavigation { get; set; }
        public Users UserIdFkNavigation { get; set; }
    }
}
