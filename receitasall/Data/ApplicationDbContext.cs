using System;
using System.Collections.Generic;
using System.Data.Entity;
using receitasall.Models;

namespace receitasall.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Cookbook> Cookbooks { get; set; }
        public DbSet<RecipeCookbook> RecipeCookbooks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Configurando o relacionamento entre Recipe e RecipeCookbook
            modelBuilder.Entity<RecipeCookbook>()
                .HasRequired(rc => rc.Recipe)
                .WithMany(r => r.RecipeCookbook)
                .HasForeignKey(rc => rc.RecipeId)
                .WillCascadeOnDelete(true); // Cascata para deletar RecipeCookbook quando Recipe for deletada

            // Configurando o relacionamento entre Cookbook e RecipeCookbook
            modelBuilder.Entity<RecipeCookbook>()
                .HasRequired(rc => rc.Cookbook)
                .WithMany(c => c.RecipeCookbook)
                .HasForeignKey(rc => rc.CookbookId)
                .WillCascadeOnDelete(false); // Não apagar Cookbook se RecipeCookbook for deletada

            // Configurando o relacionamento entre FavoriteRecipes e Recipes
            modelBuilder.Entity<FavoriteRecipe>()
                .HasRequired(fr => fr.Recipe)
                .WithMany() // Não precisa de um relacionamento inverso, se não houver
                .HasForeignKey(fr => fr.RecipeId)
                .WillCascadeOnDelete(true); // Permite deleção em cascata quando Recipe for deletada

            // Configurando o relacionamento entre FavoriteRecipes e Authors
            modelBuilder.Entity<FavoriteRecipe>()
                .HasRequired(fr => fr.Author)
                .WithMany() // Se houver um relacionamento inverso, especifique
                .HasForeignKey(fr => fr.AuthorId)
                .WillCascadeOnDelete(false); // Mantenha como false se não quiser deleção em cascata aqui

        }
    }
}
