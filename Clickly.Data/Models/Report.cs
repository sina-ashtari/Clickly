﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clickly.Data.Models
{
    public class Report
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        // Navigations 
        public Post Post { get; set; }
        public User User { get; set; }
    }
}
