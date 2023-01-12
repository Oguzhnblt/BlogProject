using BlogProject.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Entities.Entities
{
    public class Comment : CoreEntity
    {
        public string CommentText { get; set; }

        // Navigation Properties 

        [ForeignKey("User")]
        public Guid UserID { get; set; }    
        public virtual User User { get; set; }

        [ForeignKey("Post")]
        public Guid PostID { get; set; }
        public virtual Post Post { get; set; }
    }
}
