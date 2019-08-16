using System;
using System.Collections.Generic;

namespace BenchWarmerAPI.Models
{
    public partial class Users
    {
        public Users()
        {
            Characters = new HashSet<Characters>();
        }

        public int UserId { get; set; }
        public string Username { get; set; }
        public string Upassword { get; set; }

        public ICollection<Characters> Characters { get; set; }
    }
}
