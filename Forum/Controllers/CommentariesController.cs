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
    public class CommentariesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Commentaries/Create
        [Authorize(Roles = "Member")]
        public ActionResult Create(int id1,int id2)
        {
            Commentary commentary = new Commentary();
            Subject subject = new Subject();
            Category category = new Category();
            try
            {
                category = db.Categories.Find(id1);
            }
            catch //(Exception e)
            {
                TempData["danger"] = "Nu exista aceasta categorie";
                return RedirectToRoute("Categories");
                /*ViewBag.Title = "Eroare";
                ViewBag.exception = e.Message;
                ViewBag.error = "Nu exista aceasta categorie.";
                return View("~/Views/Shared/ActionError.cshtml");*/
            }
            try
            {
                subject = category.Subjects.Single(s => s.SubjectId == id2);
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
            commentary.Subject = subject;
            commentary.SubjectId = subject.SubjectId;
            commentary.UserId = User.Identity.GetUserId();
            commentary.CreateData = DateTime.Now;
            commentary.EditData = DateTime.Now;
            return View(commentary);
        }

        // POST: Commentaries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id1,int id2,[Bind(Include = "CommentaryId,Reply,UserId,SubjectId,CreateData,IsEdited,EditData")] Commentary commentary)
        {
            if (ModelState.IsValid)
            {
                db.Commentaries.Add(commentary);
                db.SaveChanges();
                TempData["message"] = "Comentariul a fost adaugat cu succes";
                return RedirectToRoute("SubjectsShow",new { id1 = id1,id2 = id2 });
            }
            return View(commentary);
        }

        // GET: Commentaries/Edit/5
        [Authorize(Roles = "Member")]
        public ActionResult Edit(int id1,int id2,int id3)
        {
            Commentary commentary = new Commentary();
            Subject subject = new Subject();
            Category category = new Category();
            try
            {
                category = db.Categories.Find(id1);
            }
            catch //(Exception e)
            {
                TempData["danger"] = "Nu exista aceasta categorie";
                return RedirectToRoute("Categories");
                /*ViewBag.Title = "Eroare";
                ViewBag.exception = e.Message;
                ViewBag.error = "Nu exista aceasta categorie.";
                return View("~/Views/Shared/ActionError.cshtml");*/
            }
            try
            {
                subject = category.Subjects.Single(s => s.SubjectId == id2);
            }
            catch //(Exception e)
            {
                TempData["danger"] = "Nu exista acest subiect";
                return RedirectToRoute("CategoriesShow", new { id = id1 });
                /*ViewBag.Title = "Eroare";
                ViewBag.exception = e.Message;
                ViewBag.error = "Nu exista acest subiect.";
                return View("~/Views/Shared/ActionError.cshtml");
                */
            }
            try
            {
                commentary = subject.Commentaries.Single(c => c.CommentaryId == id3);
            }
            catch //(Exception e)
            {
                TempData["danger"] = "Nu exista acest comentariu";
                return RedirectToRoute("SubjectsShow", new { id1 = id1, id2 = id2 });
                /*ViewBag.Title = "Eroare";
                ViewBag.exception = e.Message;
                ViewBag.error = "Nu exista acest comentariu.";
                return View("~/Views/Shared/ActionError.cshtml");
                */
            }
            if (commentary == null)
            {
                return HttpNotFound();
            }
            else if (User.Identity.GetUserId() == commentary.UserId)
            {
                commentary.EditData = DateTime.Now;
                return View(commentary);
            }
            else
            {
                TempData["danger"] = "Nu se poate modifica comentariul altui utilizator";
                return RedirectToRoute("SubjectsShow", new { id1 = id1, id2 = id2 });
                /*ViewBag.Title = "Eroare Editare";
                ViewBag.error = "Nu se poate modifica comentariul altui utilizator.";
                return View("~/Views/Shared/ActionError.cshtml");
                */
            }
        }

        // POST: Commentaries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id1,int id2,int id3,[Bind(Include = "CommentaryId,Reply,UserId,SubjectId,CreateData,IsEdited,EditData")] Commentary commentary)
        {
            if (ModelState.IsValid)
            {
                db.Entry(commentary).State = EntityState.Modified;
                db.SaveChanges();
                TempData["message"] = "Comentariul a fost modificat cu succes";
                return RedirectToRoute("SubjectsShow", new { id1, id2 });
            }
            return View(commentary);
        }

        // GET: Commentaries/Delete/5
        [Authorize(Roles = "Moderator,Member")]
        public ActionResult Delete(int id1,int id2,int id3)
        {
            Commentary commentary = new Commentary();
            Subject subject = new Subject();
            Category category = new Category();
            try
            {
                category = db.Categories.Find(id1);
            }
            catch //(Exception e)
            {
                TempData["danger"] = "Nu exista aceasta categorie";
                return RedirectToRoute("Categories");
                /*ViewBag.Title = "Eroare";
                ViewBag.exception = e.Message;
                ViewBag.error = "Nu exista aceasta categorie.";
                return View("~/Views/Shared/ActionError.cshtml");*/
            }
            try
            {
                subject = category.Subjects.Single(s => s.SubjectId == id2);
            }
            catch //(Exception e)
            {
                TempData["danger"] = "Nu exista acest subiect";
                return RedirectToRoute("CategoriesShow", new { id = id1 });
                /*ViewBag.Title = "Eroare";
                ViewBag.exception = e.Message;
                ViewBag.error = "Nu exista acest subiect.";
                return View("~/Views/Shared/ActionError.cshtml");
                */
            }
            try
            {
                commentary = subject.Commentaries.Single(c => c.CommentaryId == id3);
            }
            catch //(Exception e)
            {
                TempData["danger"] = "Nu exista acest comentariu";
                return RedirectToRoute("SubjectsShow", new { id1 = id1, id2 = id2 });
                /*ViewBag.Title = "Eroare";
                ViewBag.exception = e.Message;
                ViewBag.error = "Nu exista acest comentariu.";
                return View("~/Views/Shared/ActionError.cshtml");
                */
            }
            if (commentary == null)
            {
                return HttpNotFound();
            }
            else if (User.Identity.GetUserId() == commentary.UserId ||
                     User.IsInRole("Moderator"))
            {
                commentary.EditData = DateTime.Now;
                return View(commentary);
            }
            else
            {
                TempData["danger"] = "Nu se poate modifica comentariul altui utilizator";
                return RedirectToRoute("SubjectsShow", new { id1 = id1, id2 = id2 });
                /*ViewBag.Title = "Eroare Editare";
                ViewBag.error = "Nu se poate modifica comentariul altui utilizator.";
                return View("~/Views/Shared/ActionError.cshtml");
                */
            }
        }

        // POST: Commentaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id1,int id2,int id3)
        {
            Commentary commentary = db.Commentaries.Find(id3);
            db.Commentaries.Remove(commentary);
            db.SaveChanges();
            TempData["message"] = "Comentariul a fost sters cu succes";
            return RedirectToRoute("SubjectsShow", new { id1, id2 });
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
