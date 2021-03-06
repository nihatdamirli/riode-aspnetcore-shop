using Riode.WebUI.Migrations;
using Riode.WebUI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Models.ViewModels
{
    public class ShopFilterViewModel
    {
        public List<Brand> Brands { get; set; }
        public List<ProductColor> Colors { get; set; }
        public List<ProductSize> Sizes { get; set; }
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
    }
}
