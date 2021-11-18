using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brazo.API.Modules
{
	public class HomeModule : NancyModule
	{
		public HomeModule()
		{
			base.Get("/swagger", x =>
			{
				var site = Request.Url.SiteBase;
				return Response.AsRedirect($"/swagger-ui/dist/index.html?url={site}/api-docs");
			});
		}
	}
}
