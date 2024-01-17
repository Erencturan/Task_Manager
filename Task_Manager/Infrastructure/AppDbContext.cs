
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Task_Manager.Infrastructure.Models;


namespace Task_Manager.Infrastructure
{
    public class AppDbContext :  IdentityDbContext<User>
    {
        public DbSet<Models.Task> Tasks { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        
        }
    }

}


