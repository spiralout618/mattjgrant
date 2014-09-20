namespace mattjgrant.Migrations
{
    using mattjgrant.DAL;
using mattjgrant.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<mattjgrant.DAL.WebsiteContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(mattjgrant.DAL.WebsiteContext context)
        {
            //if (System.Diagnostics.Debugger.IsAttached == false)
            //    System.Diagnostics.Debugger.Launch();

            var rm = new RoleManager<IdentityRole>(
               new RoleStore<IdentityRole>(context));
            rm.Create(new IdentityRole("Admin"));

            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            // Create 4 test users:
            for (int i = 0; i < 4; i++)
            {
                var userName = string.Format("User{0}", i.ToString());
                var user = new ApplicationUser()
                {
                    FirstName = "User",
                    LastName = i.ToString(),
                    Email = userName + "@gmail.com",
                    UserName = userName
                };
                um.Create(user, string.Format("Password{0}", i.ToString()));
                //um.AddToRole(user.Id, "Admin");
            }

            var adminUser = new ApplicationUser()
                {
                    FirstName = "Matt",
                    LastName = "Grant",
                    Email = @"spiralout618@gmail.com",
                    UserName = "matt"
                };

            um.Create(adminUser, "Password1");

            //System.Threading.Thread.Sleep(10000);
            //Console.Write(adminUser.Id);

            um.AddToRole(adminUser.Id, "Admin");
            //This doesn't work for some reason

            //var newAdminUser = context.Users.First(u => u.UserName == "matt");
            //newAdminUser.Roles.Add(new IdentityUserRole { UserId = newAdminUser.Id, RoleId = context.Roles.First(r => r.Name == "Admin").Id });

            //context.SaveChanges();
        }
    }
}
