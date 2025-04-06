using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clickly.Data.Migrations;

namespace Clickly.Data.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        // Foregin keys 
        public int UserId { get; set; }
        public int PostId { get; set; }
        // Navigations 
        public Post Post { get; set; }
        public User User { get; set; }

        public IEnumerable<Comment> Comments { get; set; }
        public IEnumerable<Like> Likes { get; set; }


    }
}
