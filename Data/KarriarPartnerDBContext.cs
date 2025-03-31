using CC_Karriarpartner.Models;
using CC_Karriarpartner.Models;
using Microsoft.EntityFrameworkCore;

namespace CC_Karriarpartner.Data
{
    public class KarriarPartnerDBContext : DbContext
    {
            public KarriarPartnerDBContext(DbContextOptions<KarriarPartnerDBContext> options) : base(options)
            {
                
            }

            public DbSet<User> Users { get; set; }
            public DbSet<UserSubscriptions> UserSubscriptions { get; set; }
            public DbSet<Purchase> Purchases { get; set; }
            public DbSet<PurchaseItem> PurchaseItems { get; set; }
            public DbSet<Template> Templates { get; set; }
            public DbSet<Course> Courses { get; set; }
            public DbSet<Certificate> Certificates { get; set; }
            public DbSet<CourseReview> CourseReviews { get; set; }
            public DbSet<CourseVideo> CourseVideos { get; set; }


            // OnModelCreating för relationern?!

    }
}
