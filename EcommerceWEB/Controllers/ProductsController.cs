using AutoMapper;
using EcommerceAPI.DataAccess.Infrastructure;
using EcommerceAPI.Model;
using EcommerceAPI.Model.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAPI.DataAccess.EFModel;
using Microsoft.AspNetCore.Http;
using System.IO;
using EcommerceAPI.Services;
using Microsoft.EntityFrameworkCore;
using EcommerceAPI.Model.Common;

namespace EcommerceWEB.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("{controller}/{action=index}")]
    public class ProductsController : BaseController
    {
        protected readonly IWebHostEnvironment _hostingEnvironment;
        protected readonly IProductService _productService;
        public ProductsController(IProductService productService, IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment hostingEnvironment)
            : base(mapper, unitOfWork)
        {
            _hostingEnvironment = hostingEnvironment;
            _productService = productService;
        }

        [HttpGet]
        public ActionResult Index(int? pageNumber)
        {
            int pageSize = 5;
            var allProduct = _productService.GetAll().Select(p => _mapper.Map<ProductViewModel>(p)).AsQueryable();
            return View(PaginatedList<ProductViewModel>.Create(allProduct.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        public IActionResult Create()
        {
            ProductNewViewModel model = new ProductNewViewModel();
            model.Categories =  DropDownListCategoryData();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> OnPostNewProduct(ProductNewViewModel newProduct)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrEmpty(newProduct.Category))
                    {
                        if (newProduct.Categories == null) 
                        {
                            ModelState.AddModelError("", "Please select a category suitable for product");
                            newProduct.Categories = DropDownListCategoryData();
                            return View("Create", newProduct);
                        }    
                    }
                    var result = await _productService.CreateProduct(newProduct);
                    if (!result.Errored)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", result.ErrorMessage);
                        return View("Create", newProduct);
                    }
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                foreach (var modelState in ViewData.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        ModelState.AddModelError("", error.ErrorMessage);
                    }
                }
                if(newProduct.Categories == null) newProduct.Categories = DropDownListCategoryData();
                return View("Create", newProduct);
            }
        }

        [HttpGet]
        public IActionResult Update(string Id)
        {
            if (Id == null) 
                return View("Error", new ErrorViewModel("Id can not be null"));

            var productToUpdate = _productService.GetById(Id);

            if (productToUpdate == null)
                return View("Error", new ErrorViewModel("Product is not existed in Database"));

            var productToView = _mapper.Map<ProductUpdateViewModel>(productToUpdate);
            productToView.Categories = DropDownListCategoryData();
            return View(productToView);
        }

        [HttpPost]
        public async Task<IActionResult> OnPostUpdate(ProductUpdateViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _productService.Update(model);
                    if (result.Errored)
                    {
                        ModelState.AddModelError("", result.ErrorMessage);
                        return View("Create", model);
                    }

                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    throw ex;
                }
                
            }
            else
            {
                return View("Error", new ErrorViewModel("Model is valid"));
            }
        }

        public IActionResult Details(string Id)
        {
            if (!string.IsNullOrEmpty(Id))
            {
                var productDetail = _productService.GetById(Id);
                if(productDetail == null)
                    return View("Error", new ErrorViewModel("Product is not found"));

                var productToView = _mapper.Map<ProductDetailViewModel>(productDetail);
                productToView.Image = "Images/ProductImages/" + productToView.Image;
                return View(productToView);
            }
            else
            {
                return View("Error", new ErrorViewModel("Id can not null"));
            }
        }

        public IActionResult Delete(string Id)
        {
            try
            {
                if (Id == null) return View("Error", new ErrorViewModel("Id can not be null"));
                var result = _productService.Delete(Id);
                if (result.Errored)
                {
                    return View("Error", new ErrorViewModel(result.ErrorMessage));
                }
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        
        private List<SelectListItem> DropDownListCategoryData(string Id = null)
        {
            var listCate = new List<SelectListItem>();

            if(Id == null)
            {
                listCate = _unitOfwork.CategoryProductRepository.GetAll()
                                                                   .Select(x => new SelectListItem()
                                                                   {
                                                                       Value = x.Id.ToString(),
                                                                       Text = x.Name
                                                                   }).ToList();
            }
            else
            {
                listCate = _unitOfwork.CategoryProductRepository.GetAll()
                                                                   .Select(x => new SelectListItem()
                                                                   {
                                                                       Value = x.Id.ToString(),
                                                                       Text = x.Name,
                                                                       Selected = Id.Equals(x.Id.ToString()) ? true : false
                                                                   }).ToList();
            }
            
            return listCate;
        }
    }
}
