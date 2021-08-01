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

namespace EcommerceWEB.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("{controller}/{action=index}")]
    public class ProductController : BaseController
    {
        protected readonly IHostingEnvironment _hostingEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IMapper mapper, IHostingEnvironment hostingEnvironment) : base(mapper, unitOfWork)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var allProduct = _unitOfwork.ProductResponsitory.GetAll()
                                        .Select(product => _mapper.Map<ProductViewModel>(product));
            return View(allProduct);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ProductNewViewModel model = new ProductNewViewModel();
            model.Categories =  DropDownListCategoryData();
            return View(model);
        }

        [HttpPost]
        public IActionResult OnPostNewProduct(ProductNewViewModel newProduct)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Product product = new Product();
                    product.Name = newProduct.Name ?? string.Empty;
                    product.Price = Decimal.Parse(newProduct.Price);
                    product.Rating = newProduct.Rating > 5 ? 5 : newProduct.Rating;
                    product.UnitStock = newProduct.UnitStock;
                    product.Create_at = DateTime.Now;
                    Guid CateId =  Guid.Parse(newProduct.Category);
                    product.CategoryID = CateId;
                    product.Description = newProduct.Description ?? string.Empty;
                    product.Image = UploadFile(newProduct.Image) ?? "";

                    _unitOfwork.ProductResponsitory.Insert(product);
                    _unitOfwork.Save();
                    
                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                return View("Error", new ErrorViewModel("Params are invalid"));
            }
        }


        [HttpGet]
        public IActionResult Update(string Id)
        {
            if (Id == null) 
                return View("Error", new ErrorViewModel("Id can not null"));

            var productId = Guid.Parse(Id);
            var productToUpdate = _unitOfwork.ProductResponsitory.GetByID(productId);

            if(productToUpdate == null)
                return View("Error", new ErrorViewModel("Product is not existed in Database"));

            var productToView = _mapper.Map<ProductUpdateViewModel>(productToUpdate);
            productToView.Categories = DropDownListCategoryData();
            return View(productToView);
        }

        [HttpPost]
        public IActionResult OnPostUpdate(ProductUpdateViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var Id = Guid.Parse(model.Id);
                    var productToUpdate = _unitOfwork.ProductResponsitory.GetByID(Id);
                    
                    if (productToUpdate == null) return View("Error", new ErrorViewModel("User is not exists"));
                    
                    productToUpdate.Name = model.Name;
                    productToUpdate.UnitStock = model.UnitStock;
                    productToUpdate.Price = Decimal.Parse(model.Price);
                    productToUpdate.Rating = model.Rating > 5 ? 5 : model.Rating;
                    productToUpdate.Description = model.Description;
                    productToUpdate.CategoryID = Guid.Parse(model.CategoryID);
                    productToUpdate.Modify_at = DateTime.Now;
                    
                    if(model.File != null)
                    {
                        productToUpdate.Image = UploadFile(model.File);
                    }

                    _unitOfwork.ProductResponsitory.Update(productToUpdate);
                    _unitOfwork.Save();
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
                Guid id = Guid.Parse(Id);
                var productDetail = _unitOfwork.ProductResponsitory.GetByID(id);
                if(productDetail == null)
                    return View("Error", new ErrorViewModel("Product is not found"));

                var productToView = _mapper.Map<ProductDetailViewModel>(productDetail);
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
                var convertId = Guid.Parse(Id);
                var productToDelete = _unitOfwork.ProductResponsitory.GetByID(convertId);
                if (productToDelete == null)
                {
                    return View("Error", new ErrorViewModel("Product is not found"));
                }

                _unitOfwork.ProductResponsitory.Delete(productToDelete);
                _unitOfwork.Save();
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
                listCate = _unitOfwork.CategoryProductResponsitory.GetAll()
                                                                   .Select(x => new SelectListItem()
                                                                   {
                                                                       Value = x.Id.ToString(),
                                                                       Text = x.Name
                                                                   }).ToList();
            }
            else
            {
                listCate = _unitOfwork.CategoryProductResponsitory.GetAll()
                                                                   .Select(x => new SelectListItem()
                                                                   {
                                                                       Value = x.Id.ToString(),
                                                                       Text = x.Name,
                                                                       Selected = Id.Equals(x.Id.ToString()) ? true : false
                                                                   }).ToList();
            }
            
            return listCate;
        }
       private string UploadFile(IFormFile image)
       {
            if (image == null) return null;
            try
            {
                string fileName = Guid.NewGuid().ToString() + image.FileName;
                string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "Images\\ProductImages", fileName);
                var extension = new[] { "image/jpg", "image/png", "image/jpeg" };
                if (!extension.Contains(image.ContentType)) return null;
                using (FileStream file = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(file);
                }
                return fileName;
            }
            catch (Exception ex)
            {
                return null;
            }
            
       }

    }
}
