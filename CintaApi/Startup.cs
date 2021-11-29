using CintaApi.Interfaces;
using CintaApi.Models;
using CintaApi.Services;
using CommonServices.Port;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static CommonServices.Entities.Enum.ServiceTypes;

namespace CintaApi
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CintaApi", Version = "v1" });
            });


            CallApiGateway(ServiceType.Cinta, "Cinta", SetUrlGateway());


            // TODO: post al api gateway con servicetype, nombre y url


            ServiceBusMessageService.Init();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CintaApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void CallApiGateway(ServiceType serviceType, string nombre, string url)
        {
            var client = new HttpClient();

            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            keyValuePairs.Add("serviceType", serviceType.ToString());
            keyValuePairs.Add("nombre", nombre);
            keyValuePairs.Add("url", url);


            var json = JsonConvert.SerializeObject(keyValuePairs);


            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");


            client.PostAsync(url, stringContent).ConfigureAwait(false);


        }

        private static string SetUrlGateway() => $"https://localhost:5010";


    }
}
