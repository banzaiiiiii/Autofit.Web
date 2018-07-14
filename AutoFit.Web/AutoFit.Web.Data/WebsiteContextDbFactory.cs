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

		WebsiteDbContext IDesignTimeDbContextFactory<WebsiteDbContext>.CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<WebsiteDbContext>();
			optionsBuilder.UseSqlServer<WebsiteDbContext>("Server=DESKTOP-3OAMERN; Database=AutoFit-Test; Trusted_Connection=True; MultipleActiveResultSets=true;");

			return new WebsiteDbContext(optionsBuilder.Options);
		}
	}
}
