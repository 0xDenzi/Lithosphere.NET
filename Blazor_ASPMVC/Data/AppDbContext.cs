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


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>()
			.HasMany(u => u.Properties)
			.WithOne(p => p.User)
			.HasForeignKey(p => p.User)
			.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<User>()
			.HasMany(u => u.Bookmarks)
			.WithOne(b => b.User)
			.HasForeignKey(b => b.UserID);

			modelBuilder.Entity<User>()
			.HasMany(u => u.Tours)
			.WithOne(t => t.User)
			.HasForeignKey(t => t.UserID);

			modelBuilder.Entity<Property>()
			.HasMany(p => p.PropertyImages)
			.WithOne()
			.HasForeignKey(pi => pi.PropertyID)
			.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Property>()
			.HasMany(p => p.Listing)
			.WithOne(l => l.Property)
			.HasForeignKey(l => l.PropertyID);

			modelBuilder.Entity<Listing>()
			.HasMany(l => l.Bookmarks)
			.WithOne(b => b.Listing)
			.HasForeignKey(b => b.ListingID);

			modelBuilder.Entity<Listing>()
			.HasMany(l => l.Tours)
			.WithOne(t => t.Listing)
			.HasForeignKey(t => t.ListingID);

		}
	}
}
