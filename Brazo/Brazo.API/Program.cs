using Brazo.API.Host;
using Brazo.API.Logger;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nancy.Swagger.Services;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Brazo.API
{
	public class Program
	{
		public static IWebHost Host { get; set; }

		static void Main(string[] args)
		{

			/*
			string connectionString = Configuration.GetConnectionString("DemoSeriLogDB");
      var columnOptions = new ColumnOptions
      {
        AdditionalColumns = new Collection<SqlColumn>
               {
                   new SqlColumn("UserName", SqlDbType.NVarChar)
                 }
      };
      Log.Logger = new LoggerConfiguration()
          .Enrich.FromLogContext()
          .WriteTo.MSSqlServer(connectionString, sinkOptions: new MSSqlServerSinkOptions { TableName = "Log" }
          , null, null, LogEventLevel.Information, null, columnOptions: columnOptions, null, null)
          .CreateLogger();

      CreateHostBuilder(args).Build().Run();
			*/

			/*USE DB*/
			Log.Logger = new LoggerConfiguration()
				.ReadFrom
				.Configuration(Configuration)
				.CreateLogger();

			Log.Information("Starting Microservice... ");

			Log.Information($"Name [{Name}] Version [{Assembly.GetEntryAssembly().GetName().Version}]");

			var isService = !(Debugger.IsAttached || args.Contains("--console"));

			if (isService)
			{
				var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
				var pathToContentRoot = Path.GetDirectoryName(pathToExe);
				Directory.SetCurrentDirectory(pathToContentRoot);
			}
			else
			{
				Log.Information("Application started.Press Ctrl + C to shut down.");
			}

			var builder = CreateWebHostBuilder(args.Where(arg => arg != "--console").ToArray());

			var startHost = new Startup();
			startHost.Configure(builder);

			Host = builder.Build();

			ServiceStatus = ServiceStatus.Created;
			ApplicationLogging.LoggerFactory = Host.Services.GetRequiredService<ILoggerFactory>();

			var appLifetime = Host.Services.GetRequiredService<IApplicationLifetime>();
			appLifetime.ApplicationStarted.Register(() =>
			{
				Log.Information("After OnStart() has been reached");
				ServiceStatus = ServiceStatus.Started;
				AfterStart();
			});
			appLifetime.ApplicationStopping.Register(() =>
			{
				Log.Information("Calling OnStop()");
				BeforeStop();
			}
			);
			appLifetime.ApplicationStopped.Register(() => {

				Log.Information("After OnStop() has been reached");
				ServiceStatus = ServiceStatus.Stopped;
				AfterStop();
			});

			Log.Information("Calling OnStart()");
			BeforeStart();

			if (isService)
			{
				Host.RunAsCustomService();
			}
			else
			{
				Host.Run();
			}

    }

    public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
    .Build();

		public static IWebHostBuilder CreateWebHostBuilder(string[] args)
		{
			var host = new WebHostBuilder()
				.UseConfiguration(Configuration)
				.UseSerilog()
				//use healthcheck
				.UseKestrel(options =>
				{
					options.Configure(Configuration.GetSection("Kestrel"));
					options.AddServerHeader = false;
				})

				.UseStartup<Startup>()
				.SuppressStatusMessages(true);

			return host;
		}

		public static ServiceStatus ServiceStatus { get; set; }

		public static string Name => "Interfaz del Brazo";

		public static void AfterStart()
		{
			SwaggerMetadataProvider.SetInfo(
				Name,
				Assembly.GetEntryAssembly().GetName().Version.ToString(),
				"Api para acceder al Brazo del sistema"
			);

		}

		public static void AfterStop()
		{

		}

		public static void BeforeStart()
		{

		}

		public static void BeforeStop()
		{

		}


	}
}
