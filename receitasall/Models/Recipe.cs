using receitasall.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace receitasall.Models
{
    public class Recipe
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [StringLength(200)]
        [Display(Name = "Descrição")]
        public string Description { get; set; }

        [Url]
        [Display(Name = "Imagem (URL)")]
        public string Image { get; set; }

        [Required]
        [Display(Name = "Dificuldade")]
        public Difficulty Difficulty { get; set; }

        [Required]
        [Display(Name = "Receita privada")]
        public bool IsPrivate { get; set; }

        [Required]
        [Display(Name = "Tempo de preparo (minutos)")]
        public int PreparationTimeInMinutes { get; set; }

        [Required]
        [Display(Name = "Rendimento")]
        public string Rendimento { get; set; }

        [Required]
        [Display(Name = "Data de criação")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateAdded { get; set; }

        [Required]
        [Display(Name = "Data de modificação")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateUpdated { get; set; }

        [Display(Name = "Cor de destaque (Cor CSS)")]
        public string AccentColor { get; set; }

        [Required]
        public int AuthorId { get; set; }

        public virtual Author Author { get; set; }

        public virtual List<Step> Steps { get; set; } = new List<Step>();

        public virtual List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

        public virtual List<RecipeCookbook> RecipeCookbook { get; set; } = new List<RecipeCookbook>();
    }
}