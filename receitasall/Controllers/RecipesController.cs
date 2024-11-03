using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using receitasall.Models;

namespace receitasall.Controllers
{
    public class RecipesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Recipes
        public ActionResult Index()
        {
            //var userId = User.Identity.GetUserId();

            //if (userId == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Author author = db.Authors.FirstOrDefault(a => a.UserId.ToString() == userId);


            // check if its private mas pro admin mandar todos
            //var recipes = db.Recipes.Where(r => r.IsPrivate == false).ToList();

            // order by dateAdded, as ultimo adicionado vem primeiro
            var recipes = db.Recipes.OrderByDescending(r => r.DateAdded).ToList();

            return View(recipes);
        }

        // GET: Recipes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipe recipe = db.Recipes.Find(id);
            if (recipe == null)
            {
                return HttpNotFound();
            }

            ViewBag.UserAuthor = null;
            ViewBag.IsAdmin = false;

            var userId = User.Identity.GetUserId();
            if (userId != null)
            {
                // Encontrar o autor associado ao usuário logado
                Author author = db.Authors.FirstOrDefault(a => a.UserId == userId);
                ViewBag.UserAuthor = author;

                // Verificar o status de admin do usuário
                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = userManager.FindById(userId);
                if (user != null && user.Admin)
                {
                    ViewBag.IsAdmin = true;
                }
                else
                {
                    // Se a receita for privada, verifique a permissão
                    if (recipe.IsPrivate && (author == null || author.ID != recipe.AuthorId))
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                    }
                }
            } else
            {
                if (recipe.IsPrivate)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

            }


            recipe.Ingredients = db.Ingredients.Where(i => i.RecipeId == id).ToList();

            // ordena os ingredientes
            recipe.Ingredients = recipe.Ingredients.OrderBy(i => i.Order).ToList();

            return View(recipe);
        }

        // GET: Recipes/Create
        public ActionResult Create(string RedirectCookbookId)
        {

            // Inicializa o ViewModel com uma lista vazia de ingredientes
            //var viewModel = new Recipe
            //{
            //    Ingredients = new List<Ingredient>()
            //};

            var userId = User.Identity.GetUserId();

            if (userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.FirstOrDefault(a => a.UserId.ToString() == userId);

            if (author == null)
            {
                ViewBag.HasAuthor = false;
                return View();

            }
            else
            {
                ViewBag.HasAuthor = true;
                ViewBag.RedirectCookbookId = RedirectCookbookId;

                var newRecipe = new Recipe
                {
                    AuthorId = author.ID,
                    DateAdded = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    IsPrivate = true
                };

                return View(newRecipe);
            }
        }

        // POST: Recipes/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Description,Image,Difficulty,IsPrivate,PreparationTimeInMinutes,Rendimento,AccentColor")] Recipe recipe, string RedirectCookbookId)
        {
            if (ModelState.IsValid)
            {

                var userId = User.Identity.GetUserId();

                if (userId == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Author author = db.Authors.FirstOrDefault(a => a.UserId.ToString() == userId);

                recipe.AuthorId = author.ID;
                recipe.Author = author;
                recipe.DateAdded = DateTime.Now;
                recipe.DateUpdated = DateTime.Now;

                db.Recipes.Add(recipe);
                db.SaveChanges();

                if (!string.IsNullOrEmpty(RedirectCookbookId))
                {
                    return RedirectToAction("create", "recipecookbooks", new { cookbookId = RedirectCookbookId, recipeId = recipe.ID });

                }

                return RedirectToAction("Edit", new { id = recipe.ID });
            }

            ViewBag.RedirectCookbookId = RedirectCookbookId;
            return View(recipe);
        }

        // GET: Recipes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipe recipe = db.Recipes.Find(id);
            if (recipe == null)
            {
                return HttpNotFound();
            }

            // if can edit
            var userId = User.Identity.GetUserId();
            if (userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            Author author = db.Authors.FirstOrDefault(a => a.UserId.ToString() == userId);

            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindById(userId);
            if (!user.Admin)
            {
                if (author == null || author.ID != recipe.AuthorId)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

            }

            recipe.AuthorId = author.ID;
            recipe.Author = author;

            recipe.Ingredients = recipe.Ingredients.OrderBy(i => i.Order).ToList();

            return View(recipe);
        }

        // POST: Recipes/Edit/5
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Description,Image,Difficulty,IsPrivate,PreparationTimeInMinutes,Rendimento,AccentColor")] Recipe recipe)
        {
            if (ModelState.IsValid)
            {
                Recipe existingRecipe = db.Recipes.Find(recipe.ID);
                if (existingRecipe == null)
                {
                    return HttpNotFound();
                }

                var userId = User.Identity.GetUserId();
                if (userId == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Author author = db.Authors.FirstOrDefault(a => a.UserId.ToString() == userId);

                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = userManager.FindById(userId);
                if (!user.Admin)
                {
                    if (author == null || author.ID != existingRecipe.AuthorId)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                    }

                }

                // Atualiza apenas os campos que precisam ser alterados
                existingRecipe.Title = recipe.Title;
                existingRecipe.Description = recipe.Description;
                existingRecipe.Image = recipe.Image;
                existingRecipe.Difficulty = recipe.Difficulty;
                existingRecipe.IsPrivate = recipe.IsPrivate;
                existingRecipe.PreparationTimeInMinutes = recipe.PreparationTimeInMinutes;
                existingRecipe.Rendimento = recipe.Rendimento;
                existingRecipe.AccentColor = recipe.AccentColor;

                existingRecipe.DateUpdated = DateTime.Now;

                db.Entry(existingRecipe).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("edit", "Recipes", new { id = existingRecipe.ID});
                //return RedirectToAction("MyRecipes");
            }
            return View(recipe);
        }

        // GET: Recipes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipe recipe = db.Recipes.Find(id);
            if (recipe == null)
            {
                return HttpNotFound();
            }

            var userId = User.Identity.GetUserId();
            if (userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.FirstOrDefault(a => a.UserId.ToString() == userId);

            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindById(userId);
            if (!user.Admin)
            {
                if (author == null || author.ID != recipe.AuthorId)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
            }

            // verificar se tem referencias
            var recipeCookbooks = db.RecipeCookbooks.Where(rc => rc.RecipeId == recipe.ID).ToList();
            var favoriteRecipes = db.FavoriteRecipes.Where(fr => fr.RecipeId == recipe.ID).ToList();

            ViewBag.HasReferences = false;
            if (recipeCookbooks.Count > 0 || favoriteRecipes.Count > 0)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.Conflict);
                ViewBag.HasReferences = true;
            }


            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Recipe recipe = db.Recipes.Find(id);

            var userId = User.Identity.GetUserId();
            if (userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.FirstOrDefault(a => a.UserId.ToString() == userId);

            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindById(userId);
            if (!user.Admin)
            {
                if (author == null || author.ID != recipe.AuthorId)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

            }

            try
            {
                db.Recipes.Remove(recipe);
                db.SaveChanges();

            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);

            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        // GET: Recipes
        public ActionResult MyRecipes()
        {
            var userId = User.Identity.GetUserId();

            if (userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.FirstOrDefault(a => a.UserId.ToString() == userId);

            if (author == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            author.Recipes = author.Recipes.OrderByDescending(r => r.ID).ToList();

            return View(author.Recipes);
        }

        public ActionResult MyFavorites()
        {
            var userId = User.Identity.GetUserId();

            if (userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.FirstOrDefault(a => a.UserId.ToString() == userId);

            if (author == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            author.FavoriteRecipes = author.FavoriteRecipes.OrderByDescending(r => r.DateAdded).ToList();

            //var recipes = new List<Recipe>();
            //foreach (var favorite in author.FavoriteRecipes)
            //{
            //    var recipe = db.Recipes.Find(favorite.RecipeId);
            //    if (recipe != null && !recipe.IsPrivate)
            //    {
            //        recipes.Add(recipe);
            //    }
            //}

            var favoriteRecipes = author.FavoriteRecipes.Where(f => f.Recipe.IsPrivate == false).ToList();

            return View(favoriteRecipes);
        }
    }
}
