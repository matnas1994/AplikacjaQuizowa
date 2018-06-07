using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AplikacjaQuizowa.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace AplikacjaQuizowa.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ApplicationUsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;


        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: ApplicationUsers
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: ApplicationUsers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                throw new HttpException(400, "Bad Request");
            }

            ApplicationUser applicationUser = db.Users.Find(id);

            if (applicationUser == null)
            {
                throw new HttpException(400, "Bad Request");
            }

            DetailsUserModelView detailsUserModelView = new DetailsUserModelView()
            {
                Id = applicationUser.Id,
                Email = applicationUser.Email
            };

            foreach (var role in db.Roles)
            {
                CheckRoleBoxViewModel checkRoleBoxViewModel = new CheckRoleBoxViewModel()
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    Checked = false
                };

                foreach (var roleUser in role.Users)
                {
                    if (roleUser.UserId == applicationUser.Id)
                        checkRoleBoxViewModel.Checked = true;
                }
                detailsUserModelView.Roles.Add(checkRoleBoxViewModel);
            }

  

            foreach (var item in db.Score)
            {
                if (item.UserId == applicationUser)
                {
                    detailsUserModelView.Scores.Add(new Score()
                    {
                        UserId = applicationUser,
                        CategoriId = item.CategoriId,
                        Result = item.Result,
                    });

                }
            }

            return View(detailsUserModelView);
        }

        // GET: ApplicationUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                throw new HttpException(400, "Bad Request");
            }
            ApplicationUser applicationUser = db.Users.Find(id);

            

            ChangeUserModelView changeUserModelView = new ChangeUserModelView()
            {
                Id = applicationUser.Id,
                Email = applicationUser.Email,
                Password = applicationUser.PasswordHash

            };

      

            foreach (var role in db.Roles)
            {
                CheckRoleBoxViewModel checkRoleBoxViewModel = new CheckRoleBoxViewModel()
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    Checked = false
                };

                foreach (var roleUser in role.Users)
                {
                    if (roleUser.UserId == applicationUser.Id)
                        checkRoleBoxViewModel.Checked = true;
                }



                changeUserModelView.Roles.Add(checkRoleBoxViewModel);
            }

            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(changeUserModelView);
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Email,Password,Roles")] ChangeUserModelView applicationUser)
        {
            if(applicationUser.Id == null)
                throw new HttpException(400, "Bad Request");

            if (ModelState.IsValid)
            {          
                IdentityResult result;
                var user = UserManager.FindById(applicationUser.Id);

                if (applicationUser.Password != user.PasswordHash) { 
                    await UserManager.RemovePasswordAsync(applicationUser.Id);
                    result = await UserManager.AddPasswordAsync(applicationUser.Id, applicationUser.Password);          
                    if (!result.Succeeded)
                        throw new HttpException(400, "Bad Request");
                }

                if (applicationUser.Email != null)
                {
                    user.Email = applicationUser.Email;
                    user.UserName = applicationUser.Email;
                    result = await UserManager.UpdateAsync(user);

                    if (!result.Succeeded)
                        throw new HttpException(400, "Bad Request");
                }

                foreach (var role in applicationUser.Roles)
                {
                    if (role.Checked)
                    {
                        await UserManager.AddToRoleAsync(applicationUser.Id, role.RoleName);              
                    }
                    else
                    {
                        await UserManager.RemoveFromRolesAsync(applicationUser.Id, role.RoleName);
                    }
                }
                TempData["alert"] = "Zmieniłes dane użytkownika!";
                return RedirectToAction("Index");
            };
            return View(applicationUser);
        }

            // GET: ApplicationUsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                throw new HttpException(400, "Bad Request");
            }

            ApplicationUser applicationUser = db.Users.Find(id);
            ChangeUserModelView changeUserModelView = new ChangeUserModelView()
            {
                Id = applicationUser.Id,
                Email = applicationUser.Email
            };

            foreach (var role in db.Roles)
            {
                CheckRoleBoxViewModel checkRoleBoxViewModel = new CheckRoleBoxViewModel()
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    Checked = false
                };

                foreach (var roleUser in role.Users)
                {
                    if (roleUser.UserId == applicationUser.Id)
                        checkRoleBoxViewModel.Checked = true;
                }
                changeUserModelView.Roles.Add(checkRoleBoxViewModel);
            }

            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(changeUserModelView);
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (id == null)
            {
                throw new HttpException(400, "Bad Request");
            }

            ApplicationUser applicationUser = db.Users.Find(id);
            db.Users.Remove(applicationUser);
            db.SaveChanges();
            TempData["alert"] = "Usunełes użytkownika!";
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
