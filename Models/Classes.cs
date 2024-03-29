﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BenchWarmerAPI.Models
{
    public partial class Classes
    {
        public Classes()
        {
            Characters = new HashSet<Characters>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClassId { get; set; }
        public string ClassName { get; set; }

        public ICollection<Characters> Characters { get; set; }
    }
}
