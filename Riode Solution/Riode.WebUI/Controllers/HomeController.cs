using Microsoft.AspNetCore.Mvc;
using Riode.WebUI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Controllers
{
    public class HomeController : Controller
    {
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
        public IActionResult Contact(Contact model)
        {
            return View();
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
        public IActionResult Error404()
        {
            return View();
        }
        public IActionResult ComingSoon()
        {
            return View();
        }
    }
}
