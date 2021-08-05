using AutoMapper;
using EcommerceAPI.DataAccess.EFModel;
using EcommerceAPI.DataAccess.Infrastructure;
using EcommerceAPI.Model.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : BaseAPIController
    {
        public ProductsController(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork) { }

        public ActionResult Index()
        {
            var products = from p in _unitOfwork.ProductResponsitory.GetAll() join c in _unitOfwork.CategoryProductResponsitory.GetAll()
                           on p.CategoryID equals c.Id
                           select new ProductAPIModel()
                           {
                               Id = p.Id,
                               Name = p.Name,
                               Image = p.Image,
                               UnitStock = p.UnitStock,
                               Rating = p.Rating,
                               Description = p.Description,
                               Price = p.Price,
                               CategoryName = p.CategoryProduct.Name,
                               CategoryID = p.CategoryID
                           };

            return Ok(products);
        }
    }
}
