using Brazo.API.Modules;
using Brazo.Core.Contracts;
using Brazo.Core.Management;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.TinyIoc;

namespace Brazo.API.Booststrapper
{
	public class NancyBootstrapper : DefaultNancyBootstrapper
	{

    protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
    {
      base.ApplicationStartup(container, pipelines);

      var loggerFactory = Program.Host.Services.GetRequiredService<ILoggerFactory>();
      
      var defaultLogger = loggerFactory.CreateLogger("default");
      container.Register<ILogger>(defaultLogger);
      container.Register<ILoggerFactory>(loggerFactory);
      container.Register(typeof(ILogger<>), typeof(Logger<>)).AsMultiInstance();
      
      container.Register<ILogger<ServiceModule>>(
          (c, an) => loggerFactory.CreateLogger<ServiceModule>());

      container.Register(Program.Host.Services.GetRequiredService<IBrazoManagement>());
      container.Register(Program.Host.Services.GetRequiredService<GloblaSystemInformation>());
    }

    protected override void ConfigureApplicationContainer(TinyIoCContainer container)
		{
			base.ConfigureApplicationContainer(container);
		}

		protected override void ConfigureConventions(NancyConventions nancyConventions)
		{
			base.ConfigureConventions(nancyConventions);
			nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("/swagger-ui/dist"));
    }
	}
}