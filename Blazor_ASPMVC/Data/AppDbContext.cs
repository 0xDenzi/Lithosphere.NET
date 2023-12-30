using Blazor_ASPMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace Blazor_ASPMVC.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{

		}

		public DbSet<User> Users { get; set; }
		public DbSet<Property> Properties { get; set; }
		public DbSet<Bookmark> Bookmarks { get; set; }
		public DbSet<Listing> Listings { get; set; }
		public DbSet<PropertyImage> PropertyImages { get; set; }
		public DbSet<Tour> Tours { get; set; }
	}
}
