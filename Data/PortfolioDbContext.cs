using Microsoft.EntityFrameworkCore;
using netProject.Models;

namespace netProject.Data{
    // DbContext class
    public class PortfolioDbContext : DbContext {
        public PortfolioDbContext(DbContextOptions<PortfolioDbContext> options) : base(options){
            Database.EnsureCreated();

        }

        //Bind many to many relations
        protected override void OnModelCreating(ModelBuilder modelBuilder){
        modelBuilder.Entity<WebsiteLanguage>().HasKey(sc => new { sc.websiteId, sc.languageId });
        modelBuilder.Entity<WebsiteFramework>().HasKey(sc => new { sc.websiteId, sc.frameworkId });
        }

        public DbSet<Website>? Website { get; set; }
        public DbSet<Framework>? Framework { get; set; }
        public DbSet<Language>? Language { get; set; }
        public DbSet<Programs>? Programs { get; set; }
        public DbSet<Course>? Course { get; set; }
        public DbSet<WebsiteLanguage>? WebsiteLanguage { get; set; }
        public DbSet<WebsiteFramework>? WebsiteFramework { get; set; }
    }
}