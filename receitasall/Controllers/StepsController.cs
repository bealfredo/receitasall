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
    public class StepsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //// GET: Steps
        //public ActionResult Index()
        //{
        //    var steps = db.Steps.Include(s => s.Recipe);
        //    return View(steps.ToList());
        //}

        //// GET: Steps/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Step step = db.Steps.Find(id);
        //    if (step == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(step);
        //}

        // GET: Steps/Create
        [Authorize]
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

            var step = new Step
            {
                RecipeId = recipeId,
                Order = recipe.Steps.Count + 1
            };
            return View(step);
        }

        // POST: Steps/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Order,Value,RecipeId")] Step step)
        {
            if (ModelState.IsValid)
            {
                var recipe = db.Recipes.Find(step.RecipeId);
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

                step.Recipe = recipe;

                db.Steps.Add(step);
                db.SaveChanges();
                return RedirectToAction("Edit", "Recipes", new { id = step.RecipeId });
            }

            ViewBag.Recipes = db.Recipes.ToList();

            return View(step);
        }

        // GET: Steps/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Step step = db.Steps.Find(id);
            if (step == null)
            {
                return HttpNotFound();
            }

            var recipe = db.Recipes.Find(step.RecipeId);
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

            step.RecipeId = recipe.ID;
            step.Recipe = recipe;

            return View(step);
        }

        // POST: Steps/Edit/5
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Order,Value,RecipeId")] Step step)
        {
            if (ModelState.IsValid)
            {

                Step existingStep = db.Steps.Find(step.ID);
                if (existingStep == null)
                {
                    return HttpNotFound();
                }

                var recipe = db.Recipes.Find(existingStep.RecipeId);
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

                existingStep.Order = step.Order;
                existingStep.Value = step.Value;

                db.SaveChanges();
                return RedirectToAction("Edit", "Recipes", new { id = step.RecipeId });
            }
            return View(step);
        }

        // GET: Steps/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Step step = db.Steps.Find(id);
            if (step == null)
            {
                return HttpNotFound();
            }

            var recipe = db.Recipes.Find(step.RecipeId);
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

            return View(step);
        }

        // POST: Steps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Step step = db.Steps.Find(id);

            var recipe = db.Recipes.Find(step.RecipeId);
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

            db.Steps.Remove(step);
            db.SaveChanges();
            return RedirectToAction("Edit", "Recipes", new { id = step.RecipeId });
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
