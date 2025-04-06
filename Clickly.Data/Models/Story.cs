using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clickly.Data.Models
{
    public class Story
    {
        
        public int Id { get; set; }
        
        public string? Image { get; set; }
        
        public DateTime DateCreated { get; set; }
        
        public bool IsDeleted { get; set; }

        // foregin key 
        public int UserId { get; set; }
        // Navigation Properties 
        public User User { get; set; }
        // for further features, you can add these and also write them into other entities as well
        //public ICollection<Like> Like { get; set; } = new List<Like>();
        //public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        //public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
        //public ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}
