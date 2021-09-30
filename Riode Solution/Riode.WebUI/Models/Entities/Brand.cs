using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Models.Entities
{
    public class Brand : BaseEntity
    {
       
        public string Name { get; set; }
       
        public virtual ICollection<Product> Products { get; set; }
      
    }
}
