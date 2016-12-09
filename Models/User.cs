using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace theWall.Models
{
    public class User : BaseEntity
    {
        [Key]
        public int id { get; set; }
        [Required]
        [MinLength(3)]
        [Display(Name = "First Name")]
        public string first_name { get; set; }
        [Required]
        [MinLength(3)]
        [Display(Name = "Last Name")]
        public string last_name { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string email { get; set; }
        [Required]
        [MinLength(3)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string password { get; set; }
        [Required]
        [Compare("password", ErrorMessage = "Password must match confirmation")]
        [DataType(DataType.Password)]
        [Display(Name = "Password Confirmation")]
        public string confirm { get; set; }
        public DateTime created_at;
        public DateTime updated_at;
        public ICollection<Message> messages { get; set; }
        public ICollection<Comment> comments { get; set; }
    }
}