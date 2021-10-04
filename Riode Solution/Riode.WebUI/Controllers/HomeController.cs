using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Riode.WebUI.AppCode.Extensions;
using Riode.WebUI.Models.DataContexts;
using Riode.WebUI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Riode.WebUI.Controllers
{
    public class HomeController : Controller
    {
        readonly RiodeDbContext db;
        readonly IConfiguration configuration;
        public HomeController(RiodeDbContext db,IConfiguration configuration)
        {
            this.db = db;
            this.configuration = configuration;
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
                //ModelState.Clear();

                //ViewBag.Message = "Sizin sorgunuz qeyde alindi.Tezlikle size geri donulecek!";
                //return View();
                return Json(new
                {
                    error = false,
                    message = "Sizin sorgunuz qeyde alindi.Tezlikle size geri donulecek!"

                });

            }
            //return View(model);
            return Json(new
            {
                error = true,
                message = "Biraz sonra yeniden yoxlayin!"

            });

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
            var faqs = db.Faqs.Where(f => f.DeletedByUserId == null).ToList();
            return View(faqs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Subscribe([Bind("Email")]Subscribe model)
        {
            var faqs = db.Subscribes.Where(f => f.DeletedByUserId == null).ToList();
            if (ModelState.IsValid)
            {
                var current = db.Subscribes.FirstOrDefault(s => s.Email.Equals(model.Email));

                if (current != null && current.EmailConfirmed == true)
                {
                    return Json(new
                    {
                        error = true,
                        message = "Bu e-poctla daha once qeydiyyatdan kecmisiniz!"
                    });
                }

                else if (current != null && (current.EmailConfirmed ?? false == false))
                {
                    return Json(new
                    {
                        error = true,
                        message = "E-pocta gonderilmis linkle qeydiyyat tamamlanmayib"
                    });
                }

                db.Subscribes.Add(model);
                db.SaveChanges();

                string token = $"subscribetoken-{model.Id}-{DateTime.Now:yyyyMMddHHmmss}";

                token = token.Encrypte();

                string path = $"{Request.Scheme}://{Request.Host}/subscribe-confirm?token={token}";

                var MailSended = configuration.SendEmail(model.Email, "Riode Newsletter subscribe", $"Zehmet olmasa<a href={path}>Link</a>vasitesi ile abuneliyi tamamlayasiniz!");
                if (MailSended == false)
                {
                    db.Database.RollbackTransaction();
                    return Json(new
                    {
                        error = false,
                        message = "E-mail gonderilen zaman xeta bas verdi. Biraz sonra yeniden yoxlayin! "
                    });

                }


            }

            return Json(new
            {
                error = true,
                message = "Sorgunun icrasi zamani xeta yarandi!.Biraz sonra yeniden yoxlayin!"
            });
        }

        [HttpGet]
        [Route("Subscribe-Confirm")]
        public IActionResult SubscribeConfirm(string token)
        {
            token = token.Decrypto();
            Match match = Regex.Match(token, @"subscribetoken-(?<id>\d+)-(?<executeTimeStamp>\d{14})");
            if (match.Success)
            {
                int id = Convert.ToInt32(match.Groups["id"].Value);
                string executeTimeStamp = match.Groups["executeTimeStamp"].Value;
                var subscribe = db.Subscribes.FirstOrDefault(s=>s.Id==id);

                if (subscribe == null)
                {
                    ViewBag.message = Tuple.Create(true, "token xetasi");

                    goto end;
                }
                if ((subscribe.EmailConfirmed ?? false) == true)
                {
                    ViewBag.message = Tuple.Create(true, "Artiq tesdiq edilib!");

                    goto end;
                }
                subscribe.EmailConfirmed = true;
                subscribe.EmailConfirmedDate = DateTime.Now;
                db.SaveChanges();

                ViewBag.message = Tuple.Create(false, "Abuneliyiniz tesdiq edildi!");
            }
            else
            {
                ViewBag.message = Tuple.Create(true, "token xetasi");

                goto end;
            }
            end:
            return View();
        }

    }
}
