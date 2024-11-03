using receitasall.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace receitasall.Models
{
    public class Author
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 1)]
        [Display(Name = "Nome")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        [Display(Name = "Sobrenome")]
        public string LastName { get; set; }

        [Display(Name = "Nacionalidade")]
        [StringLength(50, MinimumLength = 1)]
        public string Nacionality { get; set; }

        [Display(Name = "Imagem (URL)")]
        public string Image { get; set; }

        [Display(Name = "Biografia")]
        [StringLength(500, MinimumLength = 1)]
        public string Bibliography { get; set; }

        [Display(Name = "Pseudônimo")]
        [StringLength(50, MinimumLength = 1)]
        public string Pseudonym { get; set; }

        [Display(Name = "Email de contato")]
        [EmailAddress]
        public string EmailContact { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual List<Recipe> Recipes { get; set; } = new List<Recipe>();

        public virtual List<Cookbook> Cookbooks { get; set; } = new List<Cookbook>();

        public virtual List<FavoriteRecipe> FavoriteRecipes { get; set; } = new List<FavoriteRecipe>();

    }
}