using Microsoft.EntityFrameworkCore;


namespace AutoFit.Web.Data
{
	public class WebsiteDbContext : DbContext
	{
		
		public WebsiteDbContext(DbContextOptions<WebsiteDbContext> options) : base(options)
		{
			//Database.Migrate();
		}
	
		public DbSet<Product> Products { get; set; }

	}
}

