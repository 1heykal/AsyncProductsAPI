using AsyncProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AsyncProductAPI.Data{
    public class AppDbContext : DbContext
    {

        public DbSet<ListingRequest> ListingRequests => Set<ListingRequest>();
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
      


    }
}