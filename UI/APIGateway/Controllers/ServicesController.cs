using APIGateway.Contracts;
using APIGateway.Managent;
using APIGateway.Model;
using APIGateway.Model.DTO;
using CommonServices.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CommonServices.Entities.Enum.ServiceTypes;

namespace APIGateway.Controllers
{
  [Route("api/[controller]")]
	[ApiController]
	public class ServicesController : ControllerBase
	{
		ILogger<ServicesController> _logger;

		IServiceManager _service;

		public ServicesController(ILogger<ServicesController> logger, IServiceManager service)
		{
			_logger = logger;
			_service = service;
		}

		[HttpGet]
		[Route("{type}")]
		public async Task<IList<APIServiceStatus>> GetServices(ServiceType? type)
		{
			return await _service.GetStatusServices(type);
		}

		[HttpGet]
		[Route("running/{type}")]
		public async Task<string> GetRunningService(ServiceType type)
		{
			return (await _service.GetRunningService(type)).GetName();
		}

		[HttpPost]
		public async Task AddNewService(ServiceType type, string name, string url)
		{
			await _service.AddServiceToBalancer(type, name, url);
		}

		[HttpGet]
		[Route("running/{type}/{name}/configuration")]
		public async Task<APIConfiguration> GetConfiguration(ServiceType type, string name)
		{
			return (await _service.GetConfiguration(type, name));
		}

		[HttpPost]
		[Route("running/{type}/{name}/configuration")]
		public async Task SetConfiguration(ServiceType type, string name, [FromBody]APIConfiguration configuration)
		{
			await _service.SetConfiguration(type, name, configuration);
		}



	}
}
