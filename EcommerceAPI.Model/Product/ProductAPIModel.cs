using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EcommerceAPI.Model.Product
{
    public class ProductAPIModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int UnitStock { get; set; }
        public int Rating { get; set; }
        public string Description { get; set; }
        public Decimal? Price { get; set; }
        [Required]
        public Guid CategoryID { get; set; }
    }
}
