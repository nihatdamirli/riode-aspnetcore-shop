using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Models.Entities
{
    public class Contactpost : BaseEntity
    {
        [Required(ErrorMessage = "Can't be null")]
        public string  Name { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Bu Email formata uygun deyil!")]
        public string  Email { get; set; }
        [Required]
        public string  Comment{ get; set; }
        public string  Answer{ get; set; }
        public DateTime?  AnsweredDate  { get; set; }
        public int?  AnsweredByUserId  { get; set; }
    }
}
