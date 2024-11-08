﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using receitasall.Enums;
using receitasall.Models;

namespace receitasall.Controllers
{
    public class CookbooksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Cookbooks
        //public ActionResult Index()
        //{
        //    var userId = User.Identity.GetUserId();

        //    var cookbooks = db.Cookbooks.OrderByDescending(c => c.DateAdded).ToList();

        //    return View(cookbooks);
        //}

        public ActionResult Index(string title, string authorName)
        {
            var cookbooksQuery = db.Cookbooks.Include(r => r.Author).AsQueryable();

            // Filtro por título
            if (!string.IsNullOrEmpty(title))
            {
                cookbooksQuery = cookbooksQuery.Where(r => r.Title.Contains(title));
            }


            // Filtro por nome do autor
            if (!string.IsNullOrEmpty(authorName))
            {
                authorName = authorName.Trim();
                cookbooksQuery = cookbooksQuery.Where(r =>
                    (r.Author.FirstName + " " + r.Author.LastName).Contains(authorName) ||
                    r.Author.Pseudonym.Contains(authorName)
                );
            }

            // Ordena por data de adição, mais recente primeiro
            var cookbooks = cookbooksQuery.OrderByDescending(r => r.DateAdded).ToList();

            return View(cookbooks);
        }

        // GET: Cookbooks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var cookbook = db.Cookbooks
                .Include(c => c.RecipeCookbook.Select(rc => rc.Recipe)) // Carrega Recipe associado
                .FirstOrDefault(c => c.ID == id);


            if (cookbook == null)
            {
                return HttpNotFound();
            }

            var userId = User.Identity.GetUserId();
            Author author = null;
            bool isAdmin = false;

            if (userId != null)
            {
                author = db.Authors.FirstOrDefault(a => a.UserId.ToString() == userId);
                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = userManager.FindById(userId);
                isAdmin = user.Admin;
            }

            // Se o livro for privado e o usuário não for o autor nem o administrador, negar o acesso
            if (cookbook.IsPrivate && (author == null || author.ID != cookbook.AuthorId) && !isAdmin)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            // Se o usuário for o autor ou administrador, exibir todas as receitas, incluindo privadas
            if (author != null && author.ID == cookbook.AuthorId || isAdmin)
            {
                cookbook.RecipeCookbook = cookbook.RecipeCookbook.OrderBy(i => i.Order).ToList();
            }
            else
            {
                // Filtra apenas as receitas públicas para outros usuários
                cookbook.RecipeCookbook = cookbook.RecipeCookbook
                    .Where(r => !r.Recipe.IsPrivate) // Considerando que Recipe tem a propriedade IsPrivate
                    .OrderBy(i => i.Order)
                    .ToList();
            }

            return View(cookbook);
        }

        // GET: Cookbooks/Create
        [Authorize]
        public ActionResult Create()
        {
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

                var newCookbook = new Cookbook
                {
                    AuthorId = author.ID,
                    IsPrivate = true
                };

                return View(newCookbook);
            }
        }

        // POST: Cookbooks/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Description,Image,IsPrivate,AccentColor")] Cookbook cookbook)
        {
            if (ModelState.IsValid)
            {

                var userId = User.Identity.GetUserId();

                if (userId == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Author author = db.Authors.FirstOrDefault(a => a.UserId.ToString() == userId);

                cookbook.AuthorId = author.ID;
                cookbook.Author = author;
                cookbook.DateAdded = DateTime.Now;
                cookbook.DateUpdated = DateTime.Now;

                db.Cookbooks.Add(cookbook);
                db.SaveChanges();
                return RedirectToAction("Edit", new { id = cookbook.ID });
            }

            return View(cookbook);
        }

        // GET: Cookbooks/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Usando Include para carregar as receitas dentro de RecipeCookbook
            Cookbook cookboook = db.Cookbooks
                .Include(c => c.RecipeCookbook.Select(rc => rc.Recipe)) // Inclui a entidade Recipe
                .FirstOrDefault(c => c.ID == id);
            if (cookboook == null)
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
                if (author == null || author.ID != cookboook.AuthorId)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
            }

            cookboook.AuthorId = author.ID;
            cookboook.Author = author;

            //cookboook.Ingredients = cookboook.Ingredients.OrderBy(i => i.Order).ToList();
            cookboook.RecipeCookbook = cookboook.RecipeCookbook.OrderBy(i => i.Order).ToList();

            return View(cookboook);
        }

        // POST: Cookbooks/Edit/5
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Description,Image,IsPrivate,AuthorId,AccentColor")] Cookbook cookbook)
        {
            if (ModelState.IsValid)
            {
                Cookbook existingCookbook = db.Cookbooks.Find(cookbook.ID);
                if (existingCookbook == null)
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
                    if (author == null || author.ID != existingCookbook.AuthorId)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                    }
                }

                // Atualiza apenas os campos que precisam ser alterados
                existingCookbook.Title = cookbook.Title;
                existingCookbook.Description = cookbook.Description;
                existingCookbook.Image = cookbook.Image;
                existingCookbook.IsPrivate = cookbook.IsPrivate;
                existingCookbook.AccentColor = cookbook.AccentColor;

                existingCookbook.DateUpdated = DateTime.Now;


                db.Entry(existingCookbook).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("edit", "Cookbooks", new { id = existingCookbook.ID });
            }
            return View(cookbook);
        }

        // GET: Cookbooks/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cookbook cookbook = db.Cookbooks.Find(id);
            if (cookbook == null)
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
                if (author == null || author.ID != cookbook.AuthorId)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

            }


            return View(cookbook);
        }

        // POST: Cookbooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cookbook cookbook = db.Cookbooks.Find(id);

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
                if (author == null || author.ID != cookbook.AuthorId)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

            }

            db.Cookbooks.Remove(cookbook);
            db.SaveChanges();
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
    }
}
