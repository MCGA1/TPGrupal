using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;

namespace Brazo.API.Host
{
	public static class WebHostServiceExtensions
	{
		public static void RunAsCustomService(this IWebHost host)
		{
			var webHostService = new CustomWebHostService(host);
			ServiceBase.Run(webHostService);
		}
	}
}
