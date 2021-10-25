using AutoMapper;
using EcommerceAPI.DataAccess.EFModel;
using EcommerceAPI.DataAccess.Infrastructure;
using EcommerceAPI.Model;
using EcommerceAPI.Model.CategoryProduct;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAPI.Services;

namespace EcommerceWEB.Controllers
{
    [Authorize]
    public class CategoryProductController : BaseController
    {
        protected readonly ICategoryService _categoryService;

        public CategoryProductController(ICategoryService categoryService, IUnitOfWork unitOfWork, IMapper mapper) : base(mapper, unitOfWork)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var listCateProduct = _categoryService.GetAll();
            var listCateToView = from cate in listCateProduct
                                 select new CategoryProductViewModel()
                                 {
                                     Id = cate.Id.ToString(),
                                     Name = cate.Name,
                                     Description = cate.Description,
                                     Create_at = cate.Create_at
                                 };
            return View(listCateToView);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CategoryProductNewViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var newCategory = _mapper.Map<CategoryProduct>(model);
                    var result = _categoryService.Create(newCategory);
                    if (!result.Errored)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View("Create", model);
                    }
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                ModelState.AddModelError("","Model is not valid");
                return View();
            }
        }

        [HttpGet]
        public IActionResult Update(string Id)
        {
            if (string.IsNullOrEmpty(Id))
                return View("Error", new ErrorViewModel("Id can not null"));
            try
            {
                var cate = _categoryService.GetCategoryById(Id);

                if(cate == null)
                {
                    return View("Error", new ErrorViewModel("Category is not exist"));
                }

                var cateToView = _mapper.Map<CategoryProductViewModel>(cate);

                return View(cateToView);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public IActionResult Update(CategoryProductViewModel category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Result result = _categoryService.Update(category);
                    if (result.Errored)
                    {
                        return View("Error", new ErrorViewModel(result.ErrorMessage));
                    }

                    return RedirectToAction("Update", new { Id = category.Id });
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                ModelState.AddModelError("", "Model is not valid");
                return View();
            }
        }

        [HttpGet]
        public IActionResult Delete(string Id)
        {
            try
            {
                if (Id == null)
                    return View("Error", new ErrorViewModel("Id can not null"));
                var result = _categoryService.Delete(Id);
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
    }
}
