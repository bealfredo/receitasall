using receitasall.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace receitasall.Models
{
    public class RecipeCookbook
    {
        public int ID { get; set; }
        [Required]
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        [Required]
        public int CookbookId { get; set; }
        public Cookbook Cookbook { get; set; }
        [Required]
        [Display(Name = "Prioridade de Exibição")]
        public int Order { get; set; }
        [Required]
        [Display(Name = "Data de adição")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateAdded { get; set; }

    }
}