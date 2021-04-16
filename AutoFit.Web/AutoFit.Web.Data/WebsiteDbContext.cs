using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AutoFit.Web.Data
{
	public class WebsiteDbContext : DbContext
	{
	
		public WebsiteDbContext(DbContextOptions<WebsiteDbContext> options) : base(options)
		{
		}
	
		public DbSet<Product> Products { get; set; }
		public DbSet<Stock> Stock { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderProduct> OrderProducts { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<OrderProduct>()
				.HasKey(x => new {x.ProductId, x.OrderId});
		}
	}
}

