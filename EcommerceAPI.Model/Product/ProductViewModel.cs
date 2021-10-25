using EcommerceAPI.Model.CategoryProduct;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EcommerceAPI.Model.Product
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int UnitStock { get; set; }
        public int Rating { get; set; }
        public string Price { get; set; }
        public DateTime? Create_at { get; set; }
        public DateTime? Modify_at { get; set; }
    }

    public class ProductNewViewModel
    {
        [Required]
        public string Name { get; set; }
        public IFormFile? Image { get; set; }
        public int UnitStock { get; set; }
        public int Rating { get; set; }
        [MaxLength(length:120)]
        public string Description { get; set; }
        [Required(ErrorMessage= "Price of product is required")]
        public string Price { get; set;}
        public ICollection<SelectListItem> Categories { get; set; }
        [Required(ErrorMessage ="Category is required")]
        public string Category { get; set; }
        public DateTime? Create_at { get; set; }
        public DateTime? Modify_at { get; set; }

    }

   public class ProductDetailViewModel
    {
        public string Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public int UnitStock { get; set; }
        public int Rating { get; set; }
        public string Price { get; set; }
        public string CategoryID { get; set; }
        public string Description { get; set; }
        public string Create_at { get; set; }
        public string Modify_at { get; set; }
    }

    public class ProductUpdateViewModel
    {
        public string Id { get; set; }
        public string Image { get; set; }
        public IFormFile File { get; set; }
        public string Name { get; set; }
        public int UnitStock { get; set; }
        public int Rating { get; set; }
        public string Price { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public string CategoryID { get; set; }
        public string Description { get; set; }
    }
}
