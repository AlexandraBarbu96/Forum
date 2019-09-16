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

namespace Forum.Controllers
{
    public class SubjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Subjects/Details/5
        public ActionResult Details(int id1, int id2)
        {
            Category category;
            try
            {
                //var CategoryId = int.Parse(id1);
                category = db.Categories.Find(id1);
            }
            catch //(Exception e)
            {
                TempData["danger"] = "Nu exista aceasta categorie";
                return RedirectToRoute("Categories");
                /*ViewBag.Title = "Eroare";
                ViewBag.exception = e.Message;
                ViewBag.error = "Nu exista aceasta categorie.";
                return View("~/Views/Shared/ActionError.cshtml");
                */
            }
            Subject subject;
            try
            {
                //var SubjectId = int.Parse(id2);
                subject = category.Subjects.Where(x => x.SubjectId == id2).First();
            }
            catch //(Exception e)
            {
                TempData["danger"] = "Nu exista acest subiect";
                return RedirectToRoute("CategoriesShow",new { id = id1});
                /*ViewBag.Title = "Eroare";
                ViewBag.exception = e.Message;
                ViewBag.error = "Nu exista acest subiect.";
                return View("~/Views/Shared/ActionError.cshtml");
                */
            }
            if (subject == null)
            {
                return HttpNotFound();
            }
            else
            {
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
                return View(subject);
            }
        }

        

        [Authorize(Roles = "Member")]
        public ActionResult CreateWithDropDown()
        {
            Subject subject = new Subject();
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name");
            subject.CreatorId = User.Identity.GetUserId();
            subject.CreateData = DateTime.Now;
            subject.EditData = DateTime.Now;
            subject.EditorId = User.Identity.GetUserId();
            return View(subject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateWithDropDown([Bind(Include = "SubjectId,Title,Content,CreatorId,CategoryId,CreateData,IsEdited,EditData,EditorId")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                db.Subjects.Add(subject);
                db.SaveChanges();
                TempData["message"] = "Subiectul a fost adaugat cu succes";
                return RedirectToAction("Details",new { id1 = subject.CategoryId, id2 = subject.SubjectId });
            }

            ViewBag.category_id = new SelectList(db.Categories, "CategoryId", "Name", subject.CategoryId);
            return View(subject);
        }
        
        // GET: Subjects/Create
        [Authorize(Roles = "Member")]
        public ActionResult Create(int id)
        {
            Subject subject = new Subject();
            try
            {
                subject.CategoryId = db.Categories.Find(id).CategoryId;
            }
            catch //(Exception e)
            {
                TempData["danger"] = "Nu exista aceasta categorie";
                return RedirectToRoute("Categories");
                /*ViewBag.Title = "Eroare";
                ViewBag.exception = e.Message;
                ViewBag.error = "Nu exista aceasta categorie.";
                return View("~/Views/Shared/ActionError.cshtml");
                */
            }
            subject.CreatorId = User.Identity.GetUserId();
            subject.CreateData = DateTime.Now;
            subject.EditData = DateTime.Now;
            subject.EditorId = User.Identity.GetUserId();
            return View(subject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SubjectId,Title,Content,CreatorId,CategoryId,CreateData,IsEdited,EditData,EditorId")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                db.Subjects.Add(subject);
                db.SaveChanges();
                TempData["message"] = "Subiectul a fost adaugat cu succes";
                return RedirectToAction("Details", new { id1 = subject.CategoryId, id2 = subject.SubjectId });
            }
            return View(subject);
        }

        // GET: Subjects/Edit/5
        [Authorize(Roles = "Moderator, Member")]
        public ActionResult Edit(int id1,int id2)
        {
            Category category;
            try
            {
                //var CategoryId = int.Parse(id1);
                category = db.Categories.Find(id1);
            }
            catch //(Exception e)
            {
                TempData["danger"] = "Nu exista aceasta categorie";
                return RedirectToRoute("Categories");
                /*ViewBag.Title = "Eroare";
                ViewBag.exception = e.Message;
                ViewBag.error = "Nu exista aceasta categorie.";
                return View("~/Views/Shared/ActionError.cshtml");
                */
            }
            Subject subject;
            try
            {
                //var SubjectId = int.Parse(id2);
                subject = category.Subjects.Where(x => x.SubjectId == id2).First();
            }
            catch //(Exception e)
            {
                TempData["danger"] = "Nu exista acest subiect";
                return RedirectToRoute("CategoriesShow", new { id = id1 });
                /*ViewBag.Title = "Eroare";
                ViewBag.exception = e.Message;
                ViewBag.error = "Nu exista acest subiect.";
                return View("~/Views/Shared/ActionError.cshtml");*/
            }
            if (subject == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (User.Identity.GetUserId() == subject.CreatorId)
                {
                    subject.EditData = DateTime.Now;
                    return View(subject);
                }
                else if (User.IsInRole("Moderator"))
                {
                    subject.EditorId = User.Identity.GetUserId();
                    subject.EditData = DateTime.Now;
                    ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", subject.CategoryId);
                    return View("EditModerator", subject);
                }
                else
                {
                    TempData["danger"] = "Nu se poate edita subiectul altui utilizator";
                    return RedirectToRoute("SubjectsShow",new { id1 = id1, id2 = id2});
                    /*ViewBag.Title = "Eroare Editare";
                    ViewBag.error = "Nu se poate edita subiectul altui utilizator.";
                    return View("~/Views/Shared/ActionError.cshtml");*/
                }
            }
        }

        // POST: Subjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SubjectId,Title,Content,CreatorId,CategoryId,CreateData,IsEdited,EditData,EditorId")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subject).State = EntityState.Modified;
                db.SaveChanges();
                TempData["message"] = "Subiectul a fost modificat cu succes";
                return RedirectToRoute("SubjectsShow",new { id1 = subject.CategoryId, id2 = subject.SubjectId });
            }
            if (User.IsInRole("Moderator"))
            {
                ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", subject.CategoryId);
                return View("EditModerator", subject);
            }
            return View(subject);
        }

        [Authorize(Roles = "Moderator,Member")]
        public ActionResult Delete(int id1, int id2)
        {
            Category category;
            try
            {
                category = db.Categories.Find(id1);
            }
            catch //(Exception e)
            {
                TempData["danger"] = "Nu exista aceasta categorie";
                return RedirectToRoute("Categories");
                /*ViewBag.Title = "Eroare Stergere";
                ViewBag.exception = e.Message;
                ViewBag.error = "Nu exista aceasta categorie.";
                return View("~/Views/Shared/ActionError.cshtml");
                */
            }
            Subject subject;
            try
            {
                subject = category.Subjects.Where(x => x.SubjectId == id2).First();
            }
            catch //(Exception e)
            {
                TempData["danger"] = "Nu exista acest subiect";
                return RedirectToRoute("CategoriesShow",new { id = id1 });
                /*ViewBag.Title = "Eroare Stergere";
                ViewBag.exception = e.Message;
                ViewBag.error = "Nu exista acest subiect.";
                return View("~/Views/Shared/ActionError.cshtml");
                */
            }
            if (subject == null)
            {
                return HttpNotFound();
            }
            else if (User.Identity.GetUserId() == subject.CreatorId ||
                     User.IsInRole("Moderator"))
            {
                return View(subject);
            }
            else
            {
                TempData["danger"] = "Nu se poate sterge subiectul altui utilizator";
                return RedirectToRoute("SubjectsShow", new { id1 = id1, id2 = id2 });
                /*ViewBag.Title = "Eroare Stergere";
                ViewBag.error = "Nu se poate sterge subiectul altui utilizator.";
                return View("~/Views/Shared/ActionError.cshtml");
                */
            }
        }

        // POST: Subjects/Delete/5
        [HttpDelete, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id1, int id2)
        {
            try
            {
                Subject subject = db.Categories.Find(id1).Subjects.Where(s => s.SubjectId == id2).First();
                int CategoryId = subject.CategoryId;
                db.Subjects.Remove(subject);
                db.SaveChanges();
                TempData["message"] = "Subiectul a fost sters cu succes";
                return RedirectToRoute("CategoriesShow",new { id = id1});
            }
            catch //(Exception e)
            {
                TempData["danger"] = "Nu exista acest subiect";
                return RedirectToRoute("CategoriesShow", new { id = id1 });
                /*ViewBag.Title = "Eroare Stergere";
                ViewBag.exception = e.Message;
                ViewBag.error = "Nu exista acest subiect.";
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
