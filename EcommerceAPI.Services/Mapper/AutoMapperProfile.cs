using EcommerceAPI.DataAccess.EFModel;
using EcommerceAPI.Model;
using EcommerceAPI.Model.CategoryProduct;
using EcommerceAPI.Model.Product;
using EcommerceAPI.Model.Profile;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWEB
{
    public class AutoMapperProfile : AutoMapper.Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserViewModel>();
            CreateMap<UserViewModel, User>();
            CreateMap<User, UserDetailViewModel>();
            
            CreateMap<UserEditViewModel, User> ();
            CreateMap<User, UserEditViewModel> ();
            CreateMap<UserCreateViewModel, User> ();

            CreateMap<Profile, ProfileViewModel>();
            CreateMap<ProfileViewModel,Profile> ();
            
            CreateMap<CategoryProductViewModel, CategoryProduct>();
            CreateMap<CategoryProduct, CategoryProductViewModel>();
            CreateMap<CategoryProductNewViewModel, CategoryProduct>();
            
            CreateMap<Product, ProductViewModel>();
            CreateMap<ProductNewViewModel,Product>();
            CreateMap<Product, ProductDetailViewModel>();
            CreateMap<Product, ProductUpdateViewModel>();
            CreateMap<ProductUpdateViewModel,Product>();
        }
    }
}
