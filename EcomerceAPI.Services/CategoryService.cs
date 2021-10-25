using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using EcommerceAPI.Services;
using EcommerceAPI.DataAccess.Infrastructure;
using Microsoft.AspNetCore.Identity;
using EcommerceAPI.DataAccess.EFModel;
using System.Linq;
using EcommerceAPI.Model;
using EcommerceAPI.Model.CategoryProduct;

namespace EcommerceAPI.Services
{
    public interface ICategoryService
    {
        List<CategoryProduct> GetAll();
        Result Create(CategoryProduct category);
        CategoryProduct GetCategoryById(object id);
        Result Update(CategoryProductViewModel category);
        Result Delete(object Id);
    }
    public class CategoryService : BaseService, ICategoryService
    {
        public CategoryService(IUnitOfWork unitOfWork, UserManager<User> userManager, IMapper mapper) : base(unitOfWork, userManager, mapper)
        {
        }

        public List<CategoryProduct> GetAll()
        {
            var listCateProduct = _unitOfwork.CategoryProductResponsitory.GetAll().OrderByDescending(c => c.Create_at).Distinct().ToList();
            return listCateProduct;
        }
        public Result Create(CategoryProduct category)
        {
            if (category == null)
            {
                Result error = "Params can not be null";
                return error;
            }
            _unitOfwork.CategoryProductResponsitory.Insert(category);
            _unitOfwork.Save();
            return Result.WithOutErrored;
        }
        public CategoryProduct GetCategoryById(object id)
        {
            if (id is null) return null;
            if (Guid.TryParse((string)id, out Guid Id))
            {
                var cate = _unitOfwork.CategoryProductResponsitory.GetByID(Id);
                return cate;
            }
            else
            {
                return null;
            }
        }
        public Result Update(CategoryProductViewModel category)
        {
            if (category == null)
            {
                Result error = "Params canot not null";
                return error;
            }
            var cateToUpdate = GetCategoryById(category.Id);
            if(cateToUpdate == null)
            {
                Result error = "Category is not found";
                return error;
            }
            cateToUpdate.Name = string.IsNullOrEmpty(category.Name) ? null : category.Name;
            cateToUpdate.Description = string.IsNullOrEmpty(category.Description) ? null : category.Description;
            cateToUpdate.Modify_at = DateTime.Now;

            _unitOfwork.CategoryProductResponsitory.Update(cateToUpdate);
            _unitOfwork.Save();
            return Result.WithOutErrored;
        }
        public Result Delete(object Id)
        {
            if (Id is null)
            {
                Result error = "Id can not be null";
                return error;
            }
            var cateToDelete = GetCategoryById(Id);
            if(cateToDelete == null)
            {
                Result error = "Category is not found";
                return error;
            }

            _unitOfwork.CategoryProductResponsitory.Delete(cateToDelete);
            _unitOfwork.Save();
            return Result.WithOutErrored;
        }
    }
}
