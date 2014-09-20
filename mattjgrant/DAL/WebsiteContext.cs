using mattjgrant.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Web;

namespace mattjgrant.DAL
{
    public class WebsiteContext : IdentityDbContext<ApplicationUser>
    {
        public WebsiteContext()
            : base("WebsiteContext")
        {
        }

        public DbSet<Checklist> Checklists { get; set; }
        public DbSet<ChecklistItem> ChecklistItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityUserRole>().HasKey(l => new { l.UserId, l.RoleId });
        }
    }

    //    public override int SaveChanges()
    //    {
    //        try
    //        {
    //            return base.SaveChanges();
    //        }
    //        catch (DbEntityValidationException ex)
    //        {
    //            var sb = new StringBuilder();

    //            foreach (var failure in ex.EntityValidationErrors)
    //            {
    //                sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
    //                foreach (var error in failure.ValidationErrors)
    //                {
    //                    sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
    //                    sb.AppendLine();
    //                }
    //            }

    //            throw new DbEntityValidationException(
    //                "Entity Validation Failed - errors follow:\n" +
    //                sb.ToString(), ex
    //                ); // Add the original exception as the innerException
    //        }
    //    }
    //}

    //public partial class Database : DbContext
    //{
    //    public override int SaveChanges()
    //    {
    //        try
    //        {
    //            return base.SaveChanges();
    //        }
    //        catch (DbEntityValidationException ex)
    //        {
    //            var sb = new StringBuilder();

    //            foreach (var failure in ex.EntityValidationErrors)
    //            {
    //                sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
    //                foreach (var error in failure.ValidationErrors)
    //                {
    //                    sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
    //                    sb.AppendLine();
    //                }
    //            }

    //            throw new DbEntityValidationException(
    //                "Entity Validation Failed - errors follow:\n" +
    //                sb.ToString(), ex
    //                ); // Add the original exception as the innerException
    //        }
    //    }
    //}
}