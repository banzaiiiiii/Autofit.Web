using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using AutoFit.Webf;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AutoFit.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
	        var host = BuildWebHost(args);

	        host.Run();
        }

	    public static IWebHost BuildWebHost(string[] args) =>
		    //WebHost.CreateDefaultBuilder(args)
		    //       .ConfigureAppConfiguration((context, builder) => builder.SetBasePath(context.HostingEnvironment.ContentRootPath)
		    //                                                               .AddJsonFile("appsettings.json")
		    //                                                               .Build())
		CreateBuilder(args)
		           .UseStartup<Startup>()
		           .Build();

        private static IWebHostBuilder CreateBuilder(string[] args)
        {
            var builder = new WebHostBuilder()

                .UseSetting("https_port", "443")

                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseUrls("http://localhost:5001")
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;

                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

                    if (env.IsDevelopment())
                    {
                        var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                        if (appAssembly != null)
                        {
                            config.AddUserSecrets(appAssembly, optional: true);
                        }
                    }

                    config.AddEnvironmentVariables();

                    if (args != null)
                    {
                        config.AddCommandLine(args);
                    }
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddProvider(new Log4NetProvider(GetLog4NetConfigFile()));
                })
                .UseIISIntegration()
                .UseDefaultServiceProvider((context, options) =>
                {
                    options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
                });

            return builder;
        }

        private static string GetLog4NetConfigFile()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "log4net.config");
        }
    }
}
