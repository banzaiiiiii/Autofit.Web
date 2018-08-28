using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AutoFit.Web.Data
{
	public class WebsiteContextDbFactory : IDesignTimeDbContextFactory<WebsiteDbContext>
	{
		private IConfiguration _configuration;

		public WebsiteContextDbFactory(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		WebsiteDbContext IDesignTimeDbContextFactory<WebsiteDbContext>.CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<WebsiteDbContext>();
			optionsBuilder.UseSqlServer<WebsiteDbContext>(_configuration.GetConnectionString("localDB"));

			return new WebsiteDbContext(optionsBuilder.Options);
		}
	}
}
// "Server=DESKTOP-3OAMERN; Database=AutoFit-Test; Trusted_Connection=True; MultipleActiveResultSets=true;"