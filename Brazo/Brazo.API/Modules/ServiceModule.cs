using Brazo.API.Logger;
using Brazo.Core.Contracts;
using CommonServices.Entities;
using CommonServices.Entities.Enum;
using Microsoft.Extensions.Logging;
using Nancy;
using Nancy.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brazo.API.Modules
{
	public class ServiceModule : NancyModule
	{
		private ILogger<ServiceModule> _logger;

		private IBrazoManagement _service;
		public ServiceModule(ILogger<ServiceModule> logger, IBrazoManagement service) :base("api")
		{
			_logger = logger;
			_service = service;

			Get("/", GetServiceName, name: "GetServiceName");
			Get("/configuration", GetConfiguration, name: "GetConfiguration");
			Put("/configuration", UpdateConfiguration, name: "UpdateConfiguration");
			Get("/status", GetStatus, name: "GetStatus");
			Post("/status", UpdateStatus, name: "UpdateStatus");
		}

		private object GetServiceName(dynamic arg)
		{
			_logger.LogInformation("Processing request - Get service name");
			return Response.AsJson(new
			{
				name = System.Reflection.Assembly.GetEntryAssembly().GetName().Name
			});
		}

		private async Task<object> GetConfiguration(dynamic arg)
		{
			_logger.LogInformation("Processing request - Get configuration");

			try
			{
				return Response.AsJson(await _service.GetConfiguration());
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Error getting the configuration");
				return Negotiate.WithStatusCode(HttpStatusCode.InternalServerError).WithModel(e.Message);
			}
		}

		private async Task<object> UpdateConfiguration(dynamic arg)
		{
			_logger.LogInformation("Processing request - Get configuration");

			try
			{
				var config = this.Bind<APIConfiguration>();
				await _service.UpdateConfiguration(config);

				return Negotiate.WithStatusCode(HttpStatusCode.OK);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Errpr updating configuration");
				return Negotiate.WithStatusCode(HttpStatusCode.InternalServerError).WithModel(e.Message);
			}
		}

		private async Task<object> UpdateStatus(dynamic parameters)
		{
			_logger.LogInformation("Processing request - Update status");

			try
			{
				ServiceStatus status = Enum.Parse(typeof(ServiceStatus), await ((dynamic)parameters).Value.Value);

				await _service.SetServiceStatus(status);

				return Negotiate.WithStatusCode(HttpStatusCode.OK);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Error updating status");
				return Negotiate.WithStatusCode(HttpStatusCode.InternalServerError).WithModel(e.Message);
			}
		}

		private async Task<object> GetStatus(dynamic arg)
		{
			_logger.LogInformation("Processing request - Get status");

			try
			{
				var status = await _service.GetServiceStatus();
				if(status == ServiceStatus.Stopped)
					return Negotiate.WithStatusCode(HttpStatusCode.Locked);

				return Negotiate.WithStatusCode(HttpStatusCode.OK);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Error getting status");
				return Negotiate.WithStatusCode(HttpStatusCode.InternalServerError).WithModel(e.Message);
			}
		}
	}
}
