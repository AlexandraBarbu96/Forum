using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Forum.Models
{
    public class Commentary
    {
        [Key]
        public int CommentaryId { get; set; }
        [Required(ErrorMessage = "Comentariul nu poate fi gol.")]
        public string Reply { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }

        public DateTime CreateData { get; set; }
        public bool IsEdited { get; set; }
        public DateTime EditData { get; set; }
    }
}