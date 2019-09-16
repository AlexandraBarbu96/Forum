using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Forum.Models;
using Microsoft.AspNet.Identity;
using PagedList;

namespace Forum.Controllers
{
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        public ActionResult Index()
        {
            var categories = db.Categories.OrderBy(c => c.Name).ToList();
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            if (TempData.ContainsKey("danger"))
            {
                ViewBag.message = TempData["danger"].ToString();
            }
            ViewBag.IsAdmin = false;
            ViewBag.IsModerator = false;
            ViewBag.IsMember = false;
            ViewBag.UserId = null;
            if (User.IsInRole("Administrator"))
                ViewBag.IsAdmin = true;
            if (User.IsInRole("Moderator"))
                ViewBag.IsModerator = true;
            if (User.IsInRole("Member"))
                ViewBag.IsMember = true;
            if (User.Identity.IsAuthenticated)
                ViewBag.UserId = User.Identity.GetUserId();
            return View(categories);
        }

        [NonAction]
        public ActionResult GetCategory(int id)
        {
            Category category;
            try
            {
                //var CategoryId = int.Parse(id);
                category = db.Categories.Find(id);
            }
            catch //(Exception e)
            {
                TempData["danger"] = "Nu exista aceasta categorie";
                return RedirectToAction("Index");
                /*ViewBag.Title = "Eroare";
                ViewBag.exception = e.Message;
                ViewBag.error = "Nu exista aceasta categorie.";
                return View("~/Views/Shared/ActionError.cshtml");
                */
            }
            ViewBag.IsAdmin = false;
            ViewBag.IsModerator = false;
            ViewBag.IsMember = false;
            if (User.IsInRole("Administrator"))
                ViewBag.IsAdmin = true;
            if (User.IsInRole("Moderator"))
                ViewBag.IsModerator = true;
            if (User.IsInRole("Member"))
                ViewBag.IsMember = true;
            if (User.Identity.IsAuthenticated)
                ViewBag.UserId = User.Identity.GetUserId();
            return View(category);
        }

        public ActionResult Details(int id, string sortOrder, string currentFilter, string searchString, int? page)
        {
            Category category;
            try
            {
                category = db.Categories.Find(id);
            }
            catch //(Exception e)
            {
                TempData["danger"] = "Nu exista aceasta categorie";
                return RedirectToAction("Index");
                /*ViewBag.Title = "Eroare";
                ViewBag.exception = e.Message;
                ViewBag.error = "Nu exista aceasta categorie.";
                return View("~/Views/Shared/ActionError.cshtml");
                */
            }
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            if (TempData.ContainsKey("danger"))
            {
                ViewBag.message = TempData["danger"].ToString();
            }
            ViewBag.Title = category.Name;
            ViewBag.CategoryId = category.CategoryId;
            ViewBag.IsAdmin = false;
            ViewBag.IsModerator = false;
            ViewBag.IsMember = false;
            if (User.IsInRole("Administrator"))
                ViewBag.IsAdmin = true;
            if (User.IsInRole("Moderator"))
                ViewBag.IsModerator = true;
            if (User.IsInRole("Member"))
                ViewBag.IsMember = true;
            if (User.Identity.IsAuthenticated)
                ViewBag.UserId = User.Identity.GetUserId();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "titlu_desc" : "";
            ViewBag.DateSortParm = sortOrder == "data" ? "data_desc" : "data";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var subjects = category.Subjects.AsEnumerable();
            if (!String.IsNullOrEmpty(searchString))
            {
                subjects = subjects.Where(s => s.Title.ToUpper().Contains(searchString.ToUpper())
                                       || s.Creator.UserName.ToUpper().Contains(searchString.ToUpper()));
            }
            
            switch (sortOrder)
            {
                case "titlu_desc":
                    subjects = subjects.OrderByDescending
                            (m => m.Title);
                    break;
                case "data":
                        subjects = subjects.OrderByDescending
                                (m => m.CreateData);
                    break;
                case "data_desc":
                        subjects = subjects.OrderBy
                                (m => m.CreateData);
                    break;
                case "Default":
                    subjects = subjects.OrderBy
                        (m => m.Title);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(subjects.ToPagedList(pageNumber, pageSize));
        }
        
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            Category category = new Category();
            category.AdminId = User.Identity.GetUserId();
            return View(category);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryId,Name,AdminId")]Category category)
        {
            try
            {
                db.Categories.Add(category);
                db.SaveChanges();
                TempData["message"] = "Categoria a fost adaugata cu succes";
                return RedirectToAction("Index");
            }
            catch //(Exception e)
            {
                TempData["danger"] = "Nu se poate adauga aceasta categorie";
                return RedirectToAction("Index");
                /*ViewBag.Title = "Eroare Adaugare";
                ViewBag.exception = e.Message;
                ViewBag.error = "Nu se poate adauga aceasta categorie.";
                return View("~/Views/Shared/ActionError.cshtml");*/
            }
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id)
        {
            return GetCategory(id);
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryId,Name,AdminId")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                TempData["message"] = "Categoria a fost modificata cu succes";
                return RedirectToAction("Index");
            }
            TempData["danger"] = "Nu se poate modifica aceasta categorie";
            return RedirectToAction("Index");
            /*ViewBag.Title = "Eroare Editare";
            ViewBag.error = "Nu se poate modifica aceasta categorie.";
            return View("~/Views/Shared/Error.cshtml");
            */
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            return GetCategory(id);
        }

        // POST: Categories/Delete/5
        [HttpDelete, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                //var CategoryId = int.Parse(id);
                Category categoryToDelete = db.Categories.Find(id);
                db.Categories.Remove(categoryToDelete);
                db.SaveChanges();
                TempData["message"] = "Categoria a fost stearsa cu succes";
                return RedirectToAction("Index");
            }
            catch //(Exception e)
            {
                TempData["danger"] = "Nu se poate sterge aceasta categorie";
                return RedirectToAction("Index");
                /*ViewBag.Title = "Eroare stergere";
                ViewBag.exception = e.Message;
                ViewBag.error = "Nu se poate sterge aceasta categorie.";
                return View("~/Views/Shared/ActionError.cshtml");
                */
            }
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
