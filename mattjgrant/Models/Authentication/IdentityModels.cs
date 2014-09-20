using mattjgrant.DAL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace mattjgrant.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }

    //Copy and pasted from http://typecastexception.com/post/2013/11/11/Extending-Identity-Accounts-and-Implementing-Role-Based-Authentication-in-ASPNET-MVC-5.aspx
     public class IdentityManager
    {
        public bool RoleExists(string name)
        {
            var rm = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new WebsiteContext()));
            return rm.RoleExists(name);
        }
  
  
        public bool CreateRole(string name)
        {
            var rm = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new WebsiteContext()));
            var idResult = rm.Create(new IdentityRole(name));
            return idResult.Succeeded;
        }
  
  
        public bool CreateUser(ApplicationUser user, string password)
        {
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new WebsiteContext()));
            var idResult = um.Create(user, password);
            return idResult.Succeeded;
        }
  
  
        public bool AddUserToRole(string userId, string roleName)
        {
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new WebsiteContext()));
            var idResult = um.AddToRole(userId, roleName);
            return idResult.Succeeded;
        }
  
  
        public void ClearUserRoles(string userId)
        {
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new WebsiteContext()));
            var user = um.FindById(userId);
            var currentRoles = new List<IdentityUserRole>();
            currentRoles.AddRange(user.Roles);
            foreach(var role in currentRoles)
            {
                um.RemoveFromRole(userId, role.Role.Name);
            }
        }
    }
}