using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AplikacjaQuizowa.Models;


namespace AplikacjaQuizowa.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        [Authorize(Roles = "Administrator,Moderator")]
        // GET: Categories
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        [Authorize(Roles = "Administrator,Moderator")]
        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                throw new HttpException(400, "Bad Request");
            }
            Categorie categorie = db.Categories.Find(id);
            if (categorie == null)
            {
                throw new HttpException(404, "Not found");       
            }
            return View("Details",categorie);
        }

        [Authorize(Roles = "Administrator,Moderator")]
        // GET: Categories/Create
        public ActionResult Create()
        {
            return View("Create");
        }


        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategorieId,Name")] Categorie categorie)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(categorie);
                db.SaveChanges();
                TempData["alert"] = "Stworzyłeś nowa kategorie!";
                return RedirectToAction("Index");
            }

            return View("Create",categorie);
        }

        // GET: Categories/Edit/5
        [Authorize(Roles = "Administrator,Moderator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                throw new HttpException(400, "Bad Request");
            }
            Categorie categorie = db.Categories.Find(id);
            if (categorie == null)
            {
                throw new HttpException(404, "Not found");
            }
            return View("Edit",categorie);
        }

        // POST: Categories/Edit/5
        [Authorize(Roles = "Administrator,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategorieId,Name")] Categorie categorie)
        {
            if (ModelState.IsValid)
            {
                db.Entry(categorie).State = EntityState.Modified;
                db.SaveChanges();
                TempData["alert"] = "Zmieniłes kategorie!";
                return RedirectToAction("Index");
            }
            return View(categorie);
        }

        // GET: Categories/Delete/5
        [Authorize(Roles = "Administrator,Moderator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                throw new HttpException(400, "Bad Request");
            }
            Categorie categorie = db.Categories.Find(id);
            if (categorie == null)
            {
                throw new HttpException(404, "Not found");
            }
            return View("Delete",categorie);
        }


        // POST: Categories/Delete/5
        [Authorize(Roles = "Administrator,Moderator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Categorie categorie = db.Categories.Find(id);
            db.Categories.Remove(categorie);
            db.SaveChanges();
            TempData["alert"] = "Usunołes kategorie";
            return RedirectToAction("Index");
        }

        public ActionResult SelectCategory()
        {
            return View("SelectCategory",db.Categories.ToList());
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
