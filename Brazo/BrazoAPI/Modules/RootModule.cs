using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazoAPI.Modules
{
    public class RootModule : NancyModule
    {
        public RootModule()
        {
            Get("/", _ =>
            {

                return Response.AsJson(new
                {
                    name = System.Reflection.Assembly.GetEntryAssembly().GetName().Name
                });
            });
        }
	}
}
