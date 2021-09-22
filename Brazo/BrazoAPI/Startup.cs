using Owin;
using System.Reflection;

namespace BrazoAPI
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{

			app.UseNancy();
		}
	}

}
