using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BrazoAPI
{
	class Program
	{
		static void Main(string[] args)
		{
			var urls = ConfigurationManager.AppSettings["owin:urls"].Split(';');

			var options = new StartOptions();
			foreach (var url in urls)
			{
				options.Urls.Add(url);
			}

			using (WebApp.Start<Startup>(options))
			{
				Console.WriteLine($"Running API on {ConfigurationManager.AppSettings["owin:urls"]}");
				Console.WriteLine("Press enter to exit");
				Console.ReadLine();
			}

		}
	}
}
