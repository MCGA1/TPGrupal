using Brazo.API.Logger;
using Microsoft.Extensions.Logging;
using Nancy;
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
		public ServiceModule(ILogger<ServiceModule> logger) :base("service")
		{
			_logger = logger;
			//if (_logger == null) _logger = ApplicationLogging.CreateLogger<ServiceModule>();

			Get("/", GetServiceName, name: "GetServiceName");
		}

		private object GetServiceName(dynamic arg)
		{
			_logger.LogInformation("Processing request - Get service name");
			return Response.AsJson(new
			{
				name = System.Reflection.Assembly.GetEntryAssembly().GetName().Name
			});
		}
	}
}
