﻿using BlogProject.Core.Entity;
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
        public string Title { get; set; }
        public string PostDetail { get; set; }
        public string Tags { get; set; }
        public string ImagePath { get; set; }
        public int ViewCount { get; set; }

        // Navigstion Properties
        [ForeignKey("Kategori")]
        public Guid CategoryID { get; set; }
        public virtual Category Kategori { get; set; }

        [ForeignKey("Kullanici")]
        public Guid UserID { get; set; }
        public virtual Category Kullanici { get; set; }
    }
}
