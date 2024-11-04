using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace receitasall.Models
{
    public class Ingredient
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Prioridade de Exibição")]
        public int Order { get; set; }
        [Required]
        [Display(Name = "Ingrediente")]
        [StringLength(500, MinimumLength = 1)]
        public string Value { get; set; }
        [Required]
        public int RecipeId { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}