using AutoFit.Web.Abstractions;
using AutoFit.Web.Data;
using AutoFit.Web.Data.Abstractions;
using AutoFit.Web.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace AutoFit.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
			services.AddMvc();
			services.AddAutoMapper(typeof(Startup));
			services.AddSession(options =>
			{
				options.Cookie.Name = "Cart";
				options.Cookie.MaxAge = TimeSpan.FromDays(365);
			});

			services.AddDbContext<WebsiteDbContext>(options
														=> options.UseSqlServer(Configuration.GetConnectionString("localDB")));
			services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

			services.AddScoped<IProduct, ProductService>();
	        services.AddScoped<IMail, MailService>();
	        services.AddScoped<IFileService, AzureFileService>();
			services.AddScoped<IShopService, PayPalService>();
			services.AddScoped<CartService>();

			//services.AddIdentity<IdentityUser, IdentityRole>();
			
			//services.AddHttpsRedirection(options =>
			//{
			//	options.HttpsPort = 443;
			//});

			services.Configure<CookiePolicyOptions>(options =>
			{
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
				
			});
			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(cookieOptions =>
			{
				cookieOptions.LoginPath = "/";
			});

			//services.Configure<PayPal>(Configuration.GetSection("PayPal"));

		
		}

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
				app.UseHsts();
				app.UseHttpsRedirection();
            }
			//app.UseSession();
			
			app.UseCookiePolicy();

            app.UseStaticFiles();
	        app.UseNodeModules(env.ContentRootPath);
			app.UseSession();
	        app.UseAuthentication();

            app.UseMvc(routes =>
            {
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=UnderDev}/{id?}")
					.MapRoute("AutoFit", "AutoFit/{controller=AutoFit}/{action=Index}/{id?}");  
            });
        }
    }
}
