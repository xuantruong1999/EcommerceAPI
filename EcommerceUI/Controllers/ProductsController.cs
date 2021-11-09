using AutoMapper;
using EcommerceAPI.DataAccess.EFModel;
using EcommerceAPI.DataAccess.Infrastructure;
using EcommerceAPI.Model.Product;
using EcommerceAPI.UI.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace EcommerceWEB.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : BaseController
    {
        private IConfiguration _configuration;

        public ProductsController(IHostingEnvironment hostingEnvironment, IUnitOfWork unitOfWork, IConfiguration config)
            : base(hostingEnvironment, unitOfWork)
        {
            _configuration = config;
        }

        public ActionResult Index()
        {
            //var urlImg = _httpContext.HttpContext.Request.Scheme + $"://" + _httpContext.HttpContext.Request.Host.Value + $"/Images/ProductImages/";
            var urlImg = $"http://127.0.0.1:5000/images/productimages/";
            var products = from p in _unitOfWork.ProductResponsitory.GetAll()
                           join c in _unitOfWork.CategoryProductResponsitory.GetAll()
                           on p.CategoryID equals c.Id
                           select new ProductAPIModel()
                           {
                               Id = p.Id,
                               Name = p.Name,
                               Image = urlImg + p.Image,
                               UnitStock = p.UnitStock,
                               Rating = p.Rating,
                               Description = p.Description,
                               Price = p.Price,
                               CategoryName = p.CategoryProduct.Name,
                               CategoryID = p.CategoryID,
                               CreateDate = p.Create_at,
                           };

            return Ok(products);
        }

        [Route("Details")]
        public IActionResult Details([FromQuery] string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var convertID = Guid.Parse(id);
            var product = _unitOfWork.ProductResponsitory.GetByID(convertID);

            if (product != null)
            {
                //var urlImg = _httpContext.HttpContext.Request.Scheme + $"://" + _httpContext.HttpContext.Request.Host.Value + $"/Images/ProductImages/";
                var urlImg = $"http://127.0.0.1:5000/images/productimages/";
                var category = _unitOfWork.CategoryProductResponsitory.GetByID(product.CategoryID);
                var productToView = new ProductAPIModel()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Image = urlImg + product.Image,
                    UnitStock = product.UnitStock,
                    Rating = product.Rating,
                    Description = product.Description,
                    Price = product.Price,
                    CategoryName = category.Name,
                    CategoryID = product.CategoryID
                };
                return Ok(productToView);
            }

            return NotFound();
        }
    }
}
