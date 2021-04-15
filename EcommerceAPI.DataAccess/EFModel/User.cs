using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EcommerceAPI.DataAccess.EFModel
{
    [Table("USER")]
    public class User 
    {
        public User()
        {
            Id = new Guid();
        }
        
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Profile Profile { get; set; }
    }
}
