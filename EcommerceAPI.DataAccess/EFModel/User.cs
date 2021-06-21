using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EcommerceAPI.DataAccess.EFModel
{
    [Table("User")]
    public class User
    {
        public User()
        {
            Id = new Guid();
            CreateDate = DateTime.Now;
        }

        public Guid Id { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [StringLength(60, MinimumLength = 6)]
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Phone]
        [Required]
        public string Phone { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        //SuperAdmin, Admin, Publisher, Normal User
        [Required]
        public string Permistion { get; set; }
        public Profile Profile { get; set; }
    }
}
