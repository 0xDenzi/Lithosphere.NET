using Blazor_ASPMVC.Models;
using Microsoft.EntityFrameworkCore; 

namespace Blazor_ASPMVC.Data
{
	public class AppDbContext:DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{ 

		}	

		public DbSet<testEF> tests { get; set; }
	}
}
