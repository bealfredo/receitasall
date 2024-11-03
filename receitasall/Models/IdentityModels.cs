using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace receitasall.Models
{
    // É possível adicionar dados do perfil do usuário adicionando mais propriedades na sua classe ApplicationUser, visite https://go.microsoft.com/fwlink/?LinkID=317594 para obter mais informações.
    public class ApplicationUser : IdentityUser
    {
        public bool Admin { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Observe que a authenticationType deve corresponder a uma definida em CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Adicionar declarações do usuário personalizadas aqui
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<receitasall.Models.Recipe> Recipes { get; set; }

        public System.Data.Entity.DbSet<receitasall.Models.Ingredient> Ingredients { get; set; }

        public System.Data.Entity.DbSet<receitasall.Models.Author> Authors { get; set; }

        public System.Data.Entity.DbSet<receitasall.Models.Cookbook> Cookbooks { get; set; }

        public System.Data.Entity.DbSet<receitasall.Models.RecipeCookbook> RecipeCookbooks { get; set; }

        public System.Data.Entity.DbSet<receitasall.Models.Step> Steps { get; set; }

        public System.Data.Entity.DbSet<receitasall.Models.FavoriteRecipe> FavoriteRecipes { get; set; }
    }
}