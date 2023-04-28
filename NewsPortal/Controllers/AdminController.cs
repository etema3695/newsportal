using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NewsPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsPortal.ViewModel;

namespace NewsPortal.Controllers
{
    [Authorize(Roles ="SuperAdmin")]
    public class AdminController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();

        // GET: Admin
        public ActionResult Index()
        {
            var usersWithRoles = (from user in context.Users
                                  select new
                                  {
                                      UserId = user.Id,
                                      Username = user.UserName,
                                      RoleNames = (from userRole in user.Roles
                                                   join role in context.Roles on userRole.RoleId
                                                   equals role.Id
                                                   select role.Name).ToList()
                                  }).ToList().Select(p => new UserInRoleViewModel()

                                  {
                                      UserId = p.UserId,
                                      Username = p.Username,
                                      Role = string.Join(",", p.RoleNames)
                                  });


            return View(usersWithRoles);
        }

        public ActionResult CreateUser()
        {
            ViewBag.Roles = context.Roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToList();

            return View();
        }

        [HttpPost]
        public ActionResult CreateUser(FormCollection form)
        {


            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            string usrname = form["txtEmail"];
            string email = form["txtEmail"];
            string rolname = form["RoleName"];
            string Password = form["txtPassword"];
            string Number = form["txtNumber"];

            //create default user
            var user = new ApplicationUser();
            user.UserName = usrname;
            user.Email = email;
            user.Phone = Number;
           


            var newuser = userManager.Create(user, Password);
            user = context.Users.Where(u => u.UserName.Equals(usrname, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            userManager.AddToRole(user.Id, rolname);


            return RedirectToAction("Index");
        }
    }
}