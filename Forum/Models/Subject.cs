using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Forum.Models
{
    public class Subject
    {
        [Key]
        public int SubjectId { get; set; }
        [Required(ErrorMessage = "Subiectul trebuie sa poarte un titlu.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Subiectul trebuie sa aiba un continut.")]
        public string Content { get; set; }

        [Required]
        public string CreatorId { get; set; }
        public virtual ApplicationUser Creator { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public DateTime CreateData { get; set; }
        public bool IsEdited { get; set; }
        public DateTime EditData { get; set; }

        public string EditorId { get; set; }
        public virtual ApplicationUser Editor { get; set; }

        public virtual ICollection<Commentary> Commentaries { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}