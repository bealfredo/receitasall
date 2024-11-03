using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using receitasall.Models;

namespace receitasall.Controllers
{
    public class FavoriteRecipesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //// GET: FavoriteRecipes
        //public ActionResult Index()
        //{
        //    var favoriteRecipes = db.FavoriteRecipes.Include(f => f.Author).Include(f => f.Recipe);
        //    return View(favoriteRecipes.ToList());
        //}

        // GET: FavoriteRecipes/Create
        public ActionResult Create(int recipeId)
        {
            //ViewBag.AuthorId = new SelectList(db.Authors, "ID", "FirstName");
            //ViewBag.RecipeId = new SelectList(db.Recipes, "ID", "Title");
            //return View();

            var recipe = db.Recipes.Find(recipeId);
            if (recipe == null)
            {
                return HttpNotFound();
            }
            var userId = User.Identity.GetUserId();
            Author userAuthor = db.Authors.FirstOrDefault(a => a.UserId.ToString() == userId);

            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindById(userId);
            if (!user.Admin)
            {
                if (userAuthor == null || recipe.IsPrivate)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

            }
            ViewBag.AlreadFavorited = userAuthor.FavoriteRecipes.Any(f => f.RecipeId == recipe.ID);
            ViewBag.Recipe = recipe;

            var favoriteRecipe = new FavoriteRecipe
            {
                RecipeId = recipeId,
                AuthorId = userAuthor.ID,
                //Recipe = recipe
            };
            return View(favoriteRecipe);
        }

        // POST: FavoriteRecipes/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,AuthorId,RecipeId,DateAdded")] FavoriteRecipe favoriteRecipe)
        {
            //if (ModelState.IsValid)
            //{
            //    db.FavoriteRecipes.Add(favoriteRecipe);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            //ViewBag.AuthorId = new SelectList(db.Authors, "ID", "FirstName", favoriteRecipe.AuthorId);
            //ViewBag.RecipeId = new SelectList(db.Recipes, "ID", "Title", favoriteRecipe.RecipeId);
            //return View(favoriteRecipe);

            if (ModelState.IsValid)
            {
                var recipe = db.Recipes.Find(favoriteRecipe.RecipeId);
                if (recipe == null)
                {
                    return HttpNotFound();
                }
                var userId = User.Identity.GetUserId();
                Author userAuthor = db.Authors.FirstOrDefault(a => a.UserId.ToString() == userId);

                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = userManager.FindById(userId);
                if (!user.Admin)
                {
                    if (userAuthor == null || recipe.IsPrivate)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                    }
                }

                // se alguma das receitas do usuario já existe
                var alreadFavorited = userAuthor.FavoriteRecipes.Any(f => f.RecipeId == recipe.ID);
                if (alreadFavorited)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                favoriteRecipe.Recipe = recipe;
                favoriteRecipe.DateAdded = DateTime.Now;
                favoriteRecipe.AuthorId = userAuthor.ID;
                favoriteRecipe.Author = userAuthor;
                favoriteRecipe.RecipeId = recipe.ID;


                //userAuthor.FavoriteRecipes.Add(favoriteRecipe);

                db.FavoriteRecipes.Add(favoriteRecipe);

                db.SaveChanges();

                return RedirectToAction("MyFavorites", "Recipes");
            }

            ViewBag.Recipes = db.Recipes.ToList();

            return View(favoriteRecipe);
        }



        // GET: FavoriteRecipes/Delete/5
        public ActionResult Delete(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //FavoriteRecipe favoriteRecipe = db.FavoriteRecipes.Find(id);
            //if (favoriteRecipe == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(favoriteRecipe);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FavoriteRecipe favoriteRecipe = db.FavoriteRecipes.Find(id);
            if (favoriteRecipe == null)
            {
                return HttpNotFound();
            }

            var recipe = db.Recipes.Find(favoriteRecipe.RecipeId);
            if (recipe == null)
            {
                return HttpNotFound();
            }
            var userId = User.Identity.GetUserId();
            Author userAuthor = db.Authors.FirstOrDefault(a => a.UserId.ToString() == userId);

            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindById(userId);
            if (!user.Admin)
            {
                if (userAuthor == null ||favoriteRecipe.AuthorId != userAuthor.ID)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
            }

            ViewBag.Recipe = recipe;

            return View(favoriteRecipe);
        }

        // POST: FavoriteRecipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //FavoriteRecipe favoriteRecipe = db.FavoriteRecipes.Find(id);
            //db.FavoriteRecipes.Remove(favoriteRecipe);
            //db.SaveChanges();
            //return RedirectToAction("Index");

            FavoriteRecipe favoriteRecipe = db.FavoriteRecipes.Find(id);

            var recipe = db.Recipes.Find(favoriteRecipe.RecipeId);
            if (recipe == null)
            {
                return HttpNotFound();
            }
            var userId = User.Identity.GetUserId();
            Author userAuthor = db.Authors.FirstOrDefault(a => a.UserId.ToString() == userId);

            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindById(userId);
            if (!user.Admin)
            {
                if (userAuthor == null || favoriteRecipe.AuthorId != userAuthor.ID)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
            }

            db.FavoriteRecipes.Remove(favoriteRecipe);

            db.SaveChanges();
            return RedirectToAction("MyFavorites", "Recipes");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
