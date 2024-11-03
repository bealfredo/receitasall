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
    public class IngredientsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //private bool IsOwner(Ingredient ingredient)
        //{
        //    var userId = User.Identity.GetUserId();
        //    Author userAuthor = db.Authors.FirstOrDefault(a => a.UserId.ToString() == userId);
        //    if (userAuthor == null || userAuthor.ID != ingredient.Recipe.Author.ID)
        //    {
        //        return false;
        //    }

        //    return true;
        //}


        // GET: Ingredients
        //public ActionResult Index()
        //{
        //    return View(db.Ingredients.ToList());
        //}

        // GET: Ingredients/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Ingredient ingredient = db.Ingredients.Find(id);
        //    if (ingredient == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(ingredient);
        //}

        // GET: Ingredients/Create
        public ActionResult Create(int recipeId)
        {
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
                if (userAuthor == null || userAuthor.ID != recipe.Author.ID)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

            }

            var ingredient = new Ingredient
            {
                RecipeId = recipeId,
                Order = recipe.Ingredients.Count + 1
            };
            return View(ingredient);
        }

        // POST: Ingredients/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Order,Value,RecipeId")] Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                var recipe = db.Recipes.Find(ingredient.RecipeId);
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
                    if (userAuthor == null || userAuthor.ID != recipe.Author.ID)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                    }

                }

                ingredient.Recipe = recipe;

                db.Ingredients.Add(ingredient);

                recipe.DateUpdated = DateTime.Now;
                db.Entry(recipe).State = EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction("Edit", "Recipes", new { id = ingredient.RecipeId });
            }

            ViewBag.Recipes = db.Recipes.ToList();

            return View(ingredient);
        }

        // GET: Ingredients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Ingredient ingredient = db.Ingredients.Find(id);
            if (ingredient == null)
            {
                return HttpNotFound();
            }

            var recipe = db.Recipes.Find(ingredient.RecipeId);
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
                if (userAuthor == null || userAuthor.ID != recipe.Author.ID)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
            }

            ingredient.RecipeId = recipe.ID;
            ingredient.Recipe = recipe;

            return View(ingredient);
        }

        // POST: Ingredients/Edit/5
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Order,Value,RecipeId")] Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {

                Ingredient existingIngredient = db.Ingredients.Find(ingredient.ID);
                if (existingIngredient == null)
                {
                    return HttpNotFound();
                }

                var recipe = db.Recipes.Find(existingIngredient.RecipeId);
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
                    if (userAuthor == null || userAuthor.ID != recipe.Author.ID)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                    }
                }

                existingIngredient.Order = ingredient.Order;
                existingIngredient.Value = ingredient.Value;

                recipe.DateUpdated = DateTime.Now;
                db.Entry(recipe).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Edit", "Recipes", new { id = ingredient.RecipeId });
            }
            return View(ingredient);
        }

        // GET: Ingredients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ingredient ingredient = db.Ingredients.Find(id);
            if (ingredient == null)
            {
                return HttpNotFound();
            }

            var recipe = db.Recipes.Find(ingredient.RecipeId);
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
                if (userAuthor == null || userAuthor.ID != recipe.Author.ID)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
            }

            return View(ingredient);
        }

        // POST: Ingredients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ingredient ingredient = db.Ingredients.Find(id);

            var recipe = db.Recipes.Find(ingredient.RecipeId);
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
                if (userAuthor == null || userAuthor.ID != recipe.Author.ID)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
            }

            db.Ingredients.Remove(ingredient);

            recipe.DateUpdated = DateTime.Now;
            db.Entry(recipe).State = EntityState.Modified;

            db.SaveChanges();
            return RedirectToAction("Edit", "Recipes", new { id = ingredient.RecipeId });
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
