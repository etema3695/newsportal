using Microsoft.Owin;
using Owin;
using NewsPortal.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

[assembly: OwinStartupAttribute(typeof(NewsPortal.Startup))]
namespace NewsPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //CreateUserAndRoles();
        }

        public void CreateUserAndRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!roleManager.RoleExists("SuperAdmin"))
            {
                var role = new IdentityRole("SuperAdmin");
                roleManager.Create(role);

            }

            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole("Admin");
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("Journalist"))
            {
                var role = new IdentityRole("Journalist");
                roleManager.Create(role);

            }

            if (!roleManager.RoleExists("Subscriber"))
            {
                var role = new IdentityRole("Subscriber");
                roleManager.Create(role);

            }


            //create default user
            var user = new ApplicationUser();
            user.UserName = "news@newsportal.com";
            user.Email = "news@newsportal.com";
            string Password = "Password&";

            var newuser = userManager.Create(user, Password);
            if (newuser.Succeeded)
            {
                userManager.AddToRole(user.Id,"SuperAdmin");
            }

        }
    }
}
