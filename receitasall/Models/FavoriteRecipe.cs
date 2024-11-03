using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace receitasall.Models
{
    public class FavoriteRecipe
    {
        public int ID { get; set; }

        [Required]
        public int AuthorId { get; set; }
        public virtual Author Author { get; set; }

        [Required]
        public int RecipeId { get; set; }
        public virtual Recipe Recipe { get; set; }

        [Required]
        [Display(Name = "Data favoritado")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateAdded { get; set; }
    }
}