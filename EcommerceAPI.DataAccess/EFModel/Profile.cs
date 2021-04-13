using System;
using System.Collections.Generic;
using System.Text;

namespace EcommerceAPI.DataAccess.EFModel
{
    public class Profile 
    {
        public Guid Id { get; set; }
        public string Address { get; set; }

        public User Own { get; set; }

    }
}
