using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AutoFit.Web.Data
{
	public class WebsiteDbContext : DbContext
	{
		
		public WebsiteDbContext(DbContextOptions<WebsiteDbContext> options) : base(options)
		{
			Database.EnsureCreated();
		}

		
		public DbSet<Contact> Contacts { get; set; }

	}
}

