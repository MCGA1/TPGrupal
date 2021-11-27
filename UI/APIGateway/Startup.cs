using APIGateway.Managent;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Serilog;
using System.Reflection;
using APIGateway.Contracts;
using APIGateway.Model;
using System.Text.Json.Serialization;

namespace APIGateway
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;

			Serilog.Debugging.SelfLog.Enable(msg => Trace.WriteLine(msg));

			Log.Logger = new LoggerConfiguration()
				.ReadFrom
				.Configuration(Configuration)
				.CreateLogger();

			Log.Information("Starting Microservice... ");
			Log.Information($"Name [{Assembly.GetEntryAssembly().GetName().Name}] Version [{Assembly.GetEntryAssembly().GetName().Version}]");
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{

			services.AddControllers().AddJsonOptions(opts =>
			{
				var enumConverter = new JsonStringEnumConverter();
				opts.JsonSerializerOptions.Converters.Add(enumConverter);
			});
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "APIGateway", Version = "v1" });
			});
			
			var serciveConfig = new APIGatewayConfiguration();

			Configuration.GetSection("APIGatewayConfiguration").Bind(serciveConfig);
			
			services.AddSingleton(sp => new LoadBalancer<BrazoService>(
					serciveConfig.Brazo.Select(x => new BrazoService(sp.GetRequiredService<ILogger<BrazoService>>(), x.URL, x.Name)).ToList(),
					sp.GetRequiredService<ILogger<LoadBalancer<BrazoService>>>()));

			services.AddSingleton(sp => new LoadBalancer<CintaService>(
					serciveConfig.Cinta.Select(x => new CintaService(sp.GetRequiredService<ILogger<CintaService>>(), x.URL, x.Name)).ToList(),
					sp.GetRequiredService<ILogger<LoadBalancer<CintaService>>>()));

			services.AddSingleton(sp => new LoadBalancer<PrensaService>(
					serciveConfig.Prensa.Select(x => new PrensaService(sp.GetRequiredService<ILogger<PrensaService>>(), x.URL, x.Name)).ToList(),
					sp.GetRequiredService<ILogger<LoadBalancer<PrensaService>>>()));

			services.AddSingleton<IServiceManager, ServiceManager>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			//app.UseHttpLogging();

			app.UseDeveloperExceptionPage();

			app.UseSwagger();

			app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "APIGateway v1"));

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

		}
	}
}
