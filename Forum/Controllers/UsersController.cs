using Forum.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Forum.Controllers
{
    
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            var users = db.Users.ToList();
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            if (TempData.ContainsKey("danger"))
            {
                ViewBag.message = TempData["danger"].ToString();
            }
            return View(users);
        }

        [Authorize(Roles = "Administrator,Member")]
        public ActionResult Edit(string UserName)
        {
            try
            {
                ApplicationUser user = db.Users.Single( u => u.UserName == UserName);
                var UserRole = user.Roles.FirstOrDefault().RoleId;
                ViewBag.AllRoles = new SelectList(db.Roles, "Id", "Name", UserRole);
                ViewBag.IsAdmin = false;
                ViewBag.IsMember = false;
                if (User.IsInRole("Administrator"))
                    ViewBag.IsAdmin = true;
                if (User.IsInRole("Member"))
                    ViewBag.IsMember = true;
                return View(user);
            }
            catch //(Exception e)
            {
                TempData["danger"] = "Nu exista acest utilizator";
                return RedirectToRoute("Categories");
                /*ViewBag.Title = "Eroare Utilizatori";
                ViewBag.exception = e.Message;
                ViewBag.error = "Nu exista aceast utilizator.";
                return View("~/Views/Shared/ActionError.cshtml");
                */
            }
        }
        
        [HttpPut]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string UserName, ApplicationUser NewUserData)
        {
            ApplicationUser user = db.Users.Single(u => u.UserName == UserName);
            var id = user.Id;
            var UserRole = user.Roles.FirstOrDefault().RoleId;
            ViewBag.AllRoles = new SelectList(db.Roles, "Id", "Name", UserRole);
            ViewBag.UserRole = UserRole;
            
            try
            {
                //var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

                if(TryUpdateModel(user))
                {
                    if (Request.Files.Count > 0)
                    {
                        HttpPostedFileBase upload = Request.Files["upload"];
                        var avatar = new Models.File
                        {
                            FileName = System.IO.Path.GetFileName(upload.FileName),
                            FileType = FileType.Avatar,
                            ContentType = upload.ContentType
                        };
                        using (var reader = new BinaryReader(upload.InputStream))
                        {
                            avatar.Content = reader.ReadBytes(upload.ContentLength);
                        }
                        user.Files = new List<Models.File> { avatar };
                        user.Photo = avatar.Content;
                    }
                    user.UserName = NewUserData.UserName;
                    user.Email = NewUserData.Email;
                    if (User.IsInRole("Administrator"))
                    {
                        var roles = db.Roles.ToList();

                        foreach (var role in roles)
                        {
                            UserManager.RemoveFromRole(id, role.Name);
                        }
                        var selectedRole = db.Roles.Find(HttpContext.Request.Params.Get("NewRole"));
                        UserManager.AddToRole(id, selectedRole.Name);
                    }
                    db.SaveChanges();
                }
                TempData["message"] = "Utilizatorul a fost modificat cu succes";
                
                 return RedirectToRoute("Categories");
            }
            catch//(Exception e)
            {
                TempData["danger"] = "Nu se poate modifica acest utilizator";
              
                 return RedirectToRoute("Categories");
            }
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(string UserName)
        {
            try
            {
                ApplicationUser user = db.Users.Single(u => u.UserName == UserName);
                return View(user);
            }
            catch //(Exception e)
            {
                /*ViewBag.Title = "Eroare Utilizatori";
                ViewBag.exception = e.Message;
                ViewBag.error = "Nu exista aceast utilizator.";
                return View("~/Views/Shared/ActionError.cshtml");
                */
                TempData["danger"] = "Nu exista acest utilizator";
                return RedirectToRoute("Categories");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string UserName)
        {
            try
            {
                ApplicationUser UserToDelete = db.Users.Single(u => u.UserName == UserName);
                db.Users.Remove(UserToDelete);
                db.SaveChanges();
                TempData["message"] = "utilizatorul a fost sters cu succes";
                //return View("~/Views/Categories/Index.cshmtl");
                return RedirectToRoute("Categories");
            }
            catch //(Exception e)
            {
                TempData["danger"] = "Nu exista acest utilizator";
                //return View("~/Views/Categories/Index.cshmtl");
                return RedirectToRoute("Categories");
                /*ViewBag.Title = "Eroare Utilizatori";
                ViewBag.exception = e.Message;
                ViewBag.error = "Nu exista aceast utilizator.";
                return View("~/Views/Shared/ActionError.cshtml");
                */
            }
        }

        /*public FileContentResult UserPhotos(string userId)
        {
            var userPhoto = db.Users.Where(x => x.Id == userId).FirstOrDefault().Photo;

            return new FileContentResult(userPhoto, "image/jpg");
        }
        */
    }
}