using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Models.DataContexts;
using Riode.WebUI.Models.Entities;
using Riode.WebUI.Models.FormModels;
using Riode.WebUI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Controllers
{
    public class ShopController : Controller
    {
        readonly RiodeDbContext db;
        public ShopController(RiodeDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            ShopFilterViewModel vm = new ShopFilterViewModel();

            vm.Brands = db.Brands
                .Where(b => b.DeletedByUserId == null)
                .ToList();

            vm.Colors = db.Colors
                .Where(b => b.DeletedByUserId == null)
                .ToList();


            vm.Sizes = db.Sizes
                .Where(b => b.DeletedByUserId == null)
                .ToList();

            vm.Categories = db.Categories
                .Include(c => c.Children)
                .ThenInclude(c => c.Children)
                .Where(b => b.DeletedByUserId == null && b.ParentId == null)
                .ToList();


            vm.Products = db.Products
           .Include(p => p.Images.Where(i => i.IsMain == true))
           .Include(p => p.Brand)
           .Where(p => p.DeletedByUserId == null)
           .ToList();





            return View(vm);
        }
        [HttpPost]
        public IActionResult Filter(ShopFilterFormModel model)
        {
            var query = db.Products
           .Include(p => p.Images.Where(i => i.IsMain == true))
           .Include(c => c.Brand)
           .Include(c => c.ProductSizeColorItem)
           .Where(p => p.DeletedByUserId == null)
           .AsQueryable();

            if (model?.Brands?.Count() > 0)
            {
                query = query.Where(p => model.Brands.Contains(p.BrandId));
            }

            if (model?.Sizes?.Count() > 0)
            {
                query = query
                    .Where(p =>
                    p.ProductSizeColorItem
                    .Any(psci =>model.Sizes.Contains(psci.SizeId)));
            }

            if (model?.Colors?.Count() > 0)
            {
                query = query
                    .Where(p =>
                    p.ProductSizeColorItem
                    .Any(psci => model.Colors.Contains(psci.ColorId)));
            }
            return PartialView("_ProductContainer", query.ToList());
            //return Json(new
            //{
            //    error = false,
            //    data = query.ToList()
            //});
        }
        public IActionResult Details(int id)
        {

            var product = db.Products
            .Include(p => p.Images)
            .Include(p => p.SpecificationValues.Where(s=>s.DeletedByUserId == null))
            .ThenInclude(s=>s.Specification)
            .FirstOrDefault(p => p.DeletedByUserId == null && p.Id == id);


            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
    }
}
