using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using receitasall.Models;

namespace receitasall.Controllers
{
    public class AuthorsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Authors
        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            if (userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindById(userId);
            if (!user.Admin)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            }
            return View(db.Authors.ToList());
        }

        // GET: Authors/Details/5
        [Authorize]
        public ActionResult Details()
        {
            // id frrom the user logged
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
            } else
            {
                ViewBag.HasAuthor = true;
                return View(author);
            }

        }

        // GET: Authors/Create
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

                var newAuthor = new Author
                {
                    UserId = userId.ToString() // Preenche automaticamente o RecipeId
                };

                return View(newAuthor);
            }
            else
            {
                ViewBag.HasAuthor = true;
                return View(author);
            }
        }

        // POST: Authors/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstName,LastName,Nacionality,Image,Bibliography,Pseudonym,EmailContact,UserId")] Author author)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();

                if (userId == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                bool hasAuthor = db.Authors.FirstOrDefault(a => a.UserId.ToString() == userId) != null;

                // não deixar criar outra instância de autor para o usuario
                if (hasAuthor)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                author.UserId = userId;

                db.Authors.Add(author);
                db.SaveChanges();
                return RedirectToAction("Details");
            }

            return View(author);
        }

        // GET: Authors/Edit/5
        [Authorize]
        public ActionResult Edit()
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
                return View(author);
            }
        }

        // POST: Authors/Edit/5
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,Nacionality,Image,Bibliography,Pseudonym,EmailContact,UserId")] Author author)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();

                if (userId == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Author existingAuthor = db.Authors.FirstOrDefault(a => a.UserId.ToString() == userId);

                existingAuthor.FirstName = author.FirstName;
                existingAuthor.LastName = author.LastName;
                existingAuthor.Nacionality = author.Nacionality;
                existingAuthor.Image = author.Image;
                existingAuthor.Bibliography = author.Bibliography;
                existingAuthor.Pseudonym = author.Pseudonym;
                existingAuthor.EmailContact = author.EmailContact;

                db.Entry(existingAuthor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details");
            }
            return View(author);
        }

        //// GET: Authors/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Author author = db.Authors.Find(id);
        //    if (author == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(author);
        //}

        //// POST: Authors/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Author author = db.Authors.Find(id);
        //    db.Authors.Remove(author);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
