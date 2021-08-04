using EcommerceAPI.DataAccess.Infrastructure;
using EcommerceAPI.Model.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAPI.UI.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class ProductsController : BaseController
    {
        public ProductsController(IUnitOfWork unitOfWork) : base(unitOfWork) { }
        // GET: ProductsController
        
        public ActionResult Index()
        {
            var listProduct = _unitOfWork.ProductResponsitory.GetAll().Select(p =>
            new ProductAPIModel
            {
                Id = p.Id,
                Name = p.Name,
                Image = p.Image,
                UnitStock = p.UnitStock,
                Rating = p.Rating,
                Description = p.Description,
                Price = p.Price,
                CategoryID = p.CategoryID
            });
            return Ok(listProduct.ToList());
        }

        [HttpGet("{id}")]
        // GET: ProductsController/Details/5
        public ActionResult Details(string id)
        {
            var Id = Guid.Parse(id);
            var product = _unitOfWork.ProductResponsitory.GetByID(Id);
            var productToView = new ProductAPIModel
            {
                Id = product.Id,
                Name = product.Name,
                Image = product.Image,
                UnitStock = product.UnitStock,
                Rating = product.Rating,
                Description = product.Description,
                Price = product.Price,
                CategoryID = product.CategoryID
            };
            return Ok(productToView);
        }

        // GET: ProductsController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: ProductsController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: ProductsController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        // POST: ProductsController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: ProductsController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: ProductsController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
