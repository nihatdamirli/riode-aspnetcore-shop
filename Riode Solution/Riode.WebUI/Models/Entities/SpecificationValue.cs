using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Models.Entities
{
    public class SpecificationValue:BaseEntity
    {
        public int SpecificationId { get; set; }
        public virtual Specifications Specification { get; set; }
        public int  ProductId { get; set; }
        public virtual Product Product { get; set; }
        public string Value { get; set; }

    }
}
