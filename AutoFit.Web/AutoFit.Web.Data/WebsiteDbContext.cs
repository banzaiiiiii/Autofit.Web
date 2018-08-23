using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AutoFit.Web.Data
{
	public class WebsiteDbContext : DbContext
	{
		public WebsiteDbContext(DbContextOptions<WebsiteDbContext> options) : base(options)
		{

		}

		//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		//{
		//	IConfigurationRoot configuration = new ConfigurationBuilder()
		//	                                  .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
		//	                                  .AddJsonFile("appsettings.json")
		//	                                  .Build();
		//	optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
		//}



		public DbSet<Contact> Contacts { get; set; }

	}
}

