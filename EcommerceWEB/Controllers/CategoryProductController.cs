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

namespace EcommerceWEB.Controllers
{
    [Authorize]
    public class CategoryProductController : BaseController
    {
        public CategoryProductController(IUnitOfWork unitOfWork, IMapper mapper) : base(mapper, unitOfWork)
        {
        }
        [HttpGet]
        public IActionResult Index()
        {
            var listCateProduct = _unitOfwork.CategoryProductResponsitory.GetAll();
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
                    _unitOfwork.CategoryProductResponsitory.Insert(newCategory);
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
                var id = Guid.Parse(Id);
                var cate = _unitOfwork.CategoryProductResponsitory.GetByID(id);

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
                    var cateId = Guid.Parse(category.Id);
                    var cateToUpdate = _unitOfwork.CategoryProductResponsitory.GetByID(cateId);
                    if (cateToUpdate != null)
                    {
                        cateToUpdate.Name = string.IsNullOrEmpty(category.Name) ? null : category.Name;
                        cateToUpdate.Description = string.IsNullOrEmpty(category.Description) ? null : category.Description;
                        cateToUpdate.Modify_at = DateTime.Now;

                        _unitOfwork.CategoryProductResponsitory.Update(cateToUpdate);
                        _unitOfwork.Save();
                        return RedirectToAction("Update", cateToUpdate.Id);
                    }
                    else
                    {
                        return View("Error", new ErrorViewModel("User is not existed. Maybe this user had been deleted in database"));
                    }
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

                var guidId = Guid.Parse(Id);
                var cateToDelete = _unitOfwork.CategoryProductResponsitory.GetByID(guidId);
                if (cateToDelete == null)
                    return View("Error", new ErrorViewModel("User is not existed. Maybe this user had been deleted in database"));

                _unitOfwork.CategoryProductResponsitory.Delete(cateToDelete);
                _unitOfwork.Save();

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
