using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using BVSneaker.Models.EF;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BVSneaker.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public ICollection<Order> Order { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<Blog> Blog { get; set; }
        public DbSet<Statistic> Statistic { get; set; }
        public DbSet<Brand> Brand { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductImage> ProductImage { get; set; }
        public DbSet<ProductStock> ProductStock { get; set; }
        public DbSet<Topic> Topic { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}