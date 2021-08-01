using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.DataAccess.EFModel
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int UnitStock { get; set; }
        public int Rating { get; set; }
        public string Description { get; set; }
        public Decimal Price { get; set; }
        [Required]
        public Guid CategoryID { get; set; }
        [Required]
        public CategoryProduct CategoryProduct { get; set; }
        public DateTime? Create_at { get; set; }
        public DateTime? Modify_at { get; set; }

    }
}
