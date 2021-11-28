using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nancy.Owin;
using Nancy;
using Nancy.Bootstrapper;
using Brazo.API.Booststrapper;
using Brazo.API.Logger;
using Brazo.Core.Management;
using Brazo.Core.Contracts;
using Brazo.Core.Jobs;

namespace Brazo.API
{
	public class Startup : StartupBase
	{
		public override void Configure(IApplicationBuilder app)
		{
			app.UseCors(builder =>
				builder
					.AllowAnyOrigin()
					.AllowAnyHeader()
					.AllowAnyMethod()
					.AllowCredentials()
			);

			var bootstrapper = app.ApplicationServices.GetService<INancyBootstrapper>();

			app.UseOwin(pipeline =>
			{
				pipeline.UseNancy(options => options.Bootstrapper = bootstrapper ?? new NancyBootstrapper());
			});

			var env = app.ApplicationServices.GetService<IHostingEnvironment>();

			var logger = ApplicationLogging.CreateLogger<Startup>();
			var addresses = app.ServerFeatures.Get<IServerAddressesFeature>()?.Addresses ?? Enumerable.Empty<string>();
			foreach (var address in addresses)
			{
				logger.LogInformation("Listening on [{0}]", address);
			}
			logger.LogInformation("Environment [{0}]", env.EnvironmentName);

		}

		public void Configure(IWebHostBuilder builder)
		{
			builder.ConfigureKestrel(o =>
			{
				o.AllowSynchronousIO = true;
			});

			builder.ConfigureAppConfiguration((ctx, c) =>
			{

			});

			builder.ConfigureServices((ctx, c) =>
			{
				c.AddSingleton(ctx.Configuration);
				
				c.AddCors();
				c.AddLogging((builder) =>
				{
					//builder.AddSerilog(dispose: true);
				});

				c.AddHostedService<BrazoJob>();
				c.AddSingleton<GlobalSystemInformation>();
				c.AddSingleton<IDataAccessService, DataAccessService>();
				c.AddSingleton<IBrazoManagement, BrazoManagement>();
				c.AddSingleton<IPrensaManagement, PrensaManagement>();
			});
		}

	}
}
