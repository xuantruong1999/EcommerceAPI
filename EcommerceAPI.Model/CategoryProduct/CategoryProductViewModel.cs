using EcommerceAPI.Model.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EcommerceAPI.Model.CategoryProduct
{
    public class CategoryProductViewModel
    {
        [Required]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? Create_at { get; set; }
        public DateTime? Modify_at { get; set; }
    }

    public class CategoryProductNewViewModel
    {
        public CategoryProductNewViewModel()
        {
            Create_at = DateTime.Now;
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? Create_at { get; set; }
        public DateTime? Modify_at { get; set; }
    }
}
