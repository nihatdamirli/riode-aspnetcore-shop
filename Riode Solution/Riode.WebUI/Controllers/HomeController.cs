using Microsoft.AspNetCore.Mvc;
using Riode.WebUI.Models.DataContexts;
using Riode.WebUI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Controllers
{
    public class HomeController : Controller
    {
        readonly RiodeDbContext db;
        public HomeController(RiodeDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Contact()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(Contactpost model)
        {
            ViewBag.CurrentDate = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss ffffff");
            if (ModelState.IsValid)
            {

                db.ContactPosts.Add(model);
                db.SaveChanges();
                ModelState.Clear();

                ViewBag.Message = "Sizin sorgunuz qeyde alindi.Tezlikle size geri donulecek!";
                return View();
                

            }
            return View(model);
        }
        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult MyAccount()
        {
            return View();
        }
        public IActionResult Wishlist()
        {
            return View();
        }

        public IActionResult FAQs()
        {
            return View();
        }
        
      
    }
}
