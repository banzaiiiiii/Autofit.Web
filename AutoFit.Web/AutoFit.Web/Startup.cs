﻿using System.IO;

using AutoFit.Web.Data;
using AutoFit.Web.Services;
using AutoFit.Web.ViewModels;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;


namespace AutoFit.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
	        services.AddDbContext<WebsiteDbContext>(options
		                                                => options.UseSqlServer(Configuration.GetConnectionString("localDB")));
	        services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));


	        //RegisterStores(Configuration, services);
	        //RegisterManagers(services);
	        RegisterControllerServices(services);
			
        }

	    private static void RegisterControllerServices(IServiceCollection services)
	    {
		    //services.AddScoped<HomeService>();
		    services.AddScoped<ContactService>();
			services.AddScoped<MailService>();
	    }

	    //private void RegisterManagers(IServiceCollection services)
	    //{
		   // throw new NotImplementedException();
	    //}

	    //private void RegisterStores(IConfiguration configuration, IServiceCollection services)
	    //{
		   // throw new NotImplementedException();
	    //}

	    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
	            app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
	            loggerFactory.AddLog4Net();

            }
            else
            {

                app.UseExceptionHandler("/Home/Error");
            }
			AutoMapper.Mapper.Initialize(cfg =>
			{
				cfg.CreateMap<ContactViewModel, Contact>();

			});
            app.UseStaticFiles();
	        app.UseNodeModules(env.ContentRootPath);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}")
                    .MapRoute("AutoFit", "AutoFit/{controller=AutoFit}/{action=Index}/{id?}")
		           .MapRoute("AutoService", "AutoFit/{controller=AutoService}/{action=Index}/{id?}")
		           .MapRoute("NaturKinder", "AutoFit/{controller=NaturKinder}/{action=Index}/{id?}");
            });
        }
    }
}
