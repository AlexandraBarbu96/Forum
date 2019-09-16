using Forum.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Forum.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Categoria trebuie sa poarte un nume.")]
        public string Name { get; set; }

        public string AdminId { get; set; }
        public virtual ApplicationUser Admin { get; set; }

        public virtual ICollection<Subject> Subjects { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}