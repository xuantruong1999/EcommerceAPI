﻿using AutoMapper;
using EcomerceAPI.Services;
using EcommerceAPI.DataAccess.EFModel;
using EcommerceAPI.DataAccess.Infrastructure;
using EcommerceAPI.Model;
using EcommerceAPI.Model.Product;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcommerceAPI.Services
{
    public interface IProductService
    {
        IList<Product> GetAll();
        Result CreateProduct(ProductNewViewModel newProduct);
        Product GetById(object id);
        Result Update(ProductUpdateViewModel model);
        Result Delete(string id);

    }
    public class ProductService : BaseService, IProductService
    {
        protected readonly ICommonService _commonService;
        protected readonly IHostingEnvironment _hostingEnviroment;

        public ProductService(IHostingEnvironment hostingEnvironment, ICommonService commonService,IUnitOfWork unitOfWork, UserManager<User> userManager, IMapper mapper) : base(unitOfWork, userManager, mapper)
        {
            _commonService = commonService;
            _hostingEnviroment = hostingEnvironment;
        }

        public IList<Product> GetAll()
        {
            var listProduct = _unitOfwork.ProductResponsitory.GetAll().OrderByDescending(p => p.Create_at);
            return listProduct.ToList();
        }

        public Result CreateProduct(ProductNewViewModel newProduct)
        {
            if(newProduct == null)
            {
                Result error = "Product can not be null";
                return error;
            }

            Product product = new Product();
            product.Name = newProduct.Name ?? string.Empty;
            product.Price = Decimal.Parse(newProduct.Price);
            product.Rating = newProduct.Rating > 5 ? 5 : newProduct.Rating;
            product.UnitStock = newProduct.UnitStock;
            product.Create_at = DateTime.Now;
            Guid CateId = Guid.Parse(newProduct.Category);
            product.CategoryID = CateId;
            product.Description = newProduct.Description ?? string.Empty;
            product.Image = _commonService.UploadFile(newProduct.Image) ?? "";

            _unitOfwork.ProductResponsitory.Insert(product);
            _unitOfwork.Save();

            return Result.WithOutErrored;

        }

        public Product GetById(object id)
        {
            if (id is null) return null;
            Product productToUpdate = new Product();
            if (Guid.TryParse((string)id, out Guid Id))
            {
                productToUpdate = _unitOfwork.ProductResponsitory.GetByID(Id);
            }
            return productToUpdate;
        }

        public Result Update(ProductUpdateViewModel model)
        {
            //var Id = Guid.Parse(model.Id);
            var productToUpdate = GetById(model.Id);

            if (productToUpdate == null)
            {
                Result error = "User is not exists";
                return error;
            }

            productToUpdate.Name = model.Name;
            productToUpdate.UnitStock = model.UnitStock;
            productToUpdate.Price = Decimal.Parse(model.Price);
            productToUpdate.Rating = model.Rating > 5 ? 5 : model.Rating;
            productToUpdate.Description = model.Description;
            productToUpdate.CategoryID = Guid.Parse(model.CategoryID);
            productToUpdate.Modify_at = DateTime.Now;
            string temp = productToUpdate.Image;
            if (model.File != null)
            {
                productToUpdate.Image = _commonService.UploadFile(model.File);
                if (!string.IsNullOrEmpty(temp))
                {
                   DeleteFile(temp);
                }
            }

            _unitOfwork.ProductResponsitory.Update(productToUpdate);
            _unitOfwork.Save();

            return Result.WithOutErrored;
        }
        public Result Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                Result error = "ID can not be null or empty";
                return error;
            }
            Guid.TryParse(id, out Guid Id);
            var product = _unitOfwork.ProductResponsitory.GetByID(Id);
            if (product == null)
            {
                Result error = "Product is not exist";
                return error;
            }
            _unitOfwork.ProductResponsitory.Delete(product);
            _unitOfwork.Save();
            return Result.WithOutErrored;
        }
        private void DeleteFile(string imageName)
        {
            if (imageName == null) return;

            string path = _hostingEnviroment.WebRootPath + "\\Images\\ProductImages\\" + imageName;
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            return;
        }
    }
}
