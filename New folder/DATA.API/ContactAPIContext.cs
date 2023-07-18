using DATA.API.ModelAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Model.APi.Entities;
//using Model.API.Entities;

namespace DATA.API
{
    public class ContactAPIContext: IdentityDbContext<ApplicationUser>
    {
        public DbSet<Contacts> Contacts { get; set; }

        public ContactAPIContext(DbContextOptions<ContactAPIContext> option): base(option) { }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedRoles(builder);
        }

        private void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Name = "ADMIN", ConcurrencyStamp = "1", NormalizedName = "ADMIN" },
                new IdentityRole() { Name = "USER", ConcurrencyStamp = "2", NormalizedName = "USER" });
        }
    }
}