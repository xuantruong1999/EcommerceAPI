﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EcommerceAPI.DataAccess.EFModel
{
    public class Profile 
    {
        public Profile()
        {
            Id = new Guid();
        }
        public Guid Id { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }

        public User OwnUser { get; set; }

    }
}
