using System.IO;

using AutoFit.Web.Abstractions;
using AutoFit.Web.Data;
using AutoFit.Web.Data.Abstractions;
using AutoFit.Web.Services;
using AutoFit.Web.ViewModels;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;


namespace AutoFit.Webf
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

			services.AddScoped<IContact, ContactService>();
	        services.AddScoped<IMail, MailService>();
	        services.AddScoped<IFileService, AzureFileService>();

	        services.AddAuthentication(options =>
	                 {
		                 options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
		                 options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
	                 })
	                .AddOpenIdConnect("B2C_1_sign_in", options => SetOptionsForOpenIdConnectPolicy("B2C_1_sign_in", options))
	                .AddCookie();
			services.AddHttpsRedirection(options =>
			{
				options.HttpsPort = 443;
			});

			services.Configure<CookiePolicyOptions>(options =>
			{
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});


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

			app.UseCookiePolicy();
			AutoMapper.Mapper.Initialize(cfg =>
			{
				cfg.CreateMap<ContactViewModel, Contact>();

			});
            app.UseStaticFiles();
	        app.UseNodeModules(env.ContentRootPath);

	        app.UseAuthentication();

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

	    public void SetOptionsForOpenIdConnectPolicy(string policy, OpenIdConnectOptions options)
	    {
		    options.MetadataAddress = "https://banzaii.b2clogin.com/banzaii.onmicrosoft.com/v2.0/.well-known/openid-configuration?p=" + policy;
		    options.ClientId = "efaae8de-ad2e-4a9f-9c41-a55244d2d5f1";
		    options.ResponseType = OpenIdConnectResponseType.IdToken;
		    options.CallbackPath = "/signin/" + policy;
		    options.SignedOutCallbackPath = "/signout/" + policy;
		    options.SignedOutRedirectUri = "/";
		    options.TokenValidationParameters.NameClaimType = "name";
	    }
    }
}
