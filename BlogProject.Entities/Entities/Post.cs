using BlogProject.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Entities.Entities
{
    public class Post : CoreEntity
    {
        public Post()
        {
            this.Comments = new List<Comment>();
        }
        public string Title { get; set; }
        public string PostDetail { get; set; }
        public string Tags { get; set; }
        public string? ImagePath { get; set; }
        public int ViewCount { get; set; }

        // Navigation Properties
        [ForeignKey("Category")]
        public Guid CategoryID { get; set; }
        public virtual Category? Category { get; set; }

        [ForeignKey("User")]
        public Guid UserID { get; set; }
        public virtual User? User { get; set; }
        public virtual List<Comment> Comments { get; set; }


    }
}
