using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace theWall.Models
{
    public class Message : BaseEntity
    {
        [Key]
        public int id { get; set; }
        [Required]
        [MinLength(3)]
        public string content { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public User user { get; set; }
        // public ICollection<Comment> comments { get; set; }
    }
}