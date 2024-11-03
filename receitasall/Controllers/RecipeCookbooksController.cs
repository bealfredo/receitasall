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
    public class RecipeCookbooksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //// GET: RecipeCookbooks
        //public ActionResult Index()
        //{
        //    var recipeCookbooks = db.RecipeCookbooks.Include(r => r.Cookbook).Include(r => r.Recipe);
        //    return View(recipeCookbooks.ToList());
        //}// to delete

        //// GET: RecipeCookbooks/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    RecipeCookbook recipeCookbook = db.RecipeCookbooks.Find(id);
        //    if (recipeCookbook == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(recipeCookbook);
        //}// to delete

        // GET: RecipeCookbooks/Create
        public ActionResult Create(int cookbookId, int? recipeId)
        {
            var cookbook = db.Cookbooks.Find(cookbookId);
            if (cookbook == null)
            {
                return HttpNotFound();
            }
            var recipe = db.Recipes.Find(recipeId);

            var userId = User.Identity.GetUserId();
            Author userAuthor = db.Authors.FirstOrDefault(a => a.UserId.ToString() == userId);

            var recipeCookbook = new RecipeCookbook
            {
                CookbookId = cookbookId,
                Order = cookbook.RecipeCookbook.Count + 1,
            };

            if (recipeId != null)
            {
                if (recipe == null)
                {
                    return HttpNotFound();
                }

                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = userManager.FindById(userId);
                if (!user.Admin)
                {
                    if (userAuthor == null || userAuthor.ID != recipe.Author.ID)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                    }

                }
                recipeCookbook.RecipeId = (int)recipeId;
            }

            var recipeIdsInCookbook = cookbook.RecipeCookbook.Select(rc => rc.RecipeId).ToList();

            var userRecipes = db.Recipes
                .Where(r => r.Author.ID == userAuthor.ID && !recipeIdsInCookbook.Contains(r.ID))
                .ToList();

            ViewBag.UserRecipes = userRecipes;

            ViewBag.RecipeId = new SelectList(userRecipes, "ID", "Title");

            ViewBag.Recipe = recipe;
            ViewBag.Cookbook = cookbook;

            return View(recipeCookbook);
        }

        // POST: RecipeCookbooks/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,RecipeId,CookbookId,Order")] RecipeCookbook recipeCookbook)
        {
            if (ModelState.IsValid)
            {
                var cookbook = db.Cookbooks.Find(recipeCookbook.CookbookId);
                if (cookbook == null)
                {
                    return HttpNotFound();
                }

                var recipe = db.Recipes.Find(recipeCookbook.RecipeId);
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
                    if (userAuthor == null || userAuthor.ID != cookbook.Author.ID)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                    }

                    if (recipe.Author.ID != userAuthor.ID)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                    }

                }

                recipeCookbook.Cookbook = cookbook;
                recipeCookbook.Recipe = recipe;
                recipeCookbook.DateAdded = DateTime.Now;

                db.RecipeCookbooks.Add(recipeCookbook);
                db.SaveChanges();
                return RedirectToAction("Edit", "cookbooks", new { id = recipeCookbook.CookbookId });
            }

            ViewBag.RecipeId = new SelectList(db.Recipes, "ID", "Title");

            return View(recipeCookbook);
        }

        // GET: RecipeCookbooks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            RecipeCookbook recipeCookbook = db.RecipeCookbooks.Find(id);
            if (recipeCookbook == null)
            {
                return HttpNotFound();
            }

            var cookbook = db.Cookbooks.Find(recipeCookbook.CookbookId);
            if (cookbook == null)
            {
                return HttpNotFound();
            }

            var recipe = db.Recipes.Find(recipeCookbook.RecipeId);
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
                if (userAuthor == null || userAuthor.ID != cookbook.Author.ID)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

            }

            ViewBag.Recipe = recipe;
            ViewBag.Cookbook = cookbook;

            return View(recipeCookbook);
        }

        // POST: RecipeCookbooks/Edit/5
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID, Order")] RecipeCookbook recipeCookbook)
        {
            if (ModelState.IsValid)
            {
                RecipeCookbook existingRecipeCookbook = db.RecipeCookbooks.Find(recipeCookbook.ID);
                if (existingRecipeCookbook == null)
                {
                    return HttpNotFound();
                }

                var cookbook = db.Cookbooks.Find(existingRecipeCookbook.CookbookId);
                if (cookbook == null)
                {
                    return HttpNotFound();
                }

                var recipe = db.Recipes.Find(existingRecipeCookbook.RecipeId);
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
                    if (userAuthor == null || userAuthor.ID != cookbook.Author.ID)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                    }

                }

                existingRecipeCookbook.Order = recipeCookbook.Order;

                db.Entry(existingRecipeCookbook).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "cookbooks", new { id = existingRecipeCookbook.CookbookId });
            }

            return View(recipeCookbook);
        }

        // GET: RecipeCookbooks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecipeCookbook recipeCookbook = db.RecipeCookbooks.Find(id);
            if (recipeCookbook == null)
            {
                return HttpNotFound();
            }

            var cookbook = db.Cookbooks.Find(recipeCookbook.CookbookId);
            if (cookbook == null)
            {
                return HttpNotFound();
            }

            var recipe = db.Recipes.Find(recipeCookbook.RecipeId);
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
                if (userAuthor == null || userAuthor.ID != cookbook.Author.ID)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

            }

            ViewBag.Cookbook = cookbook;
            ViewBag.Recipe = recipe;


            return View(recipeCookbook);
        }

        // POST: RecipeCookbooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RecipeCookbook recipeCookbook = db.RecipeCookbooks.Find(id);

            var cookbook = db.Cookbooks.Find(recipeCookbook.CookbookId);
            if (cookbook == null)
            {
                return HttpNotFound();
            }
            var userId = User.Identity.GetUserId();
            Author userAuthor = db.Authors.FirstOrDefault(a => a.UserId.ToString() == userId);

            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindById(userId);
            if (!user.Admin)
            {
                if (userAuthor == null || userAuthor.ID != cookbook.Author.ID)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

            }

            db.RecipeCookbooks.Remove(recipeCookbook);
            db.SaveChanges();
            //return RedirectToAction("Index");
            return RedirectToAction("Edit", "cookbooks", new { id = cookbook.ID });
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
