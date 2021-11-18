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
		public ServiceModule() :base("service")
		{
			Get("/", GetServiceName, name: "GetServiceName");
		}

		private object GetServiceName(dynamic arg)
		{
			return Response.AsJson(new
			{
				name = System.Reflection.Assembly.GetEntryAssembly().GetName().Name
			});
		}
	}
}
