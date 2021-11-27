using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UserInterface.Models;

namespace UserInterface
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

		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews();
			services.Configure<ApiGatewayConfiguration>(Configuration.GetSection("ApiConfiguration"));

			services.AddHttpClient("APIGatewayService");
			services.AddSingleton(ctx =>
			{
				var clientFactory = ctx.GetRequiredService<IHttpClientFactory>();
				var httpClient = clientFactory.CreateClient("APIGatewayService");

				return new APIGatewayService(ctx.GetRequiredService<IOptions<ApiGatewayConfiguration>>().Value.APIUrl, httpClient);
			});

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
									name: "default",
									pattern: "{controller=Home}/{action=Index}/{id?}");
			});
			
		}
	}
}
