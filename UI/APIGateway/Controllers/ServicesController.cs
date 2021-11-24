using APIGateway.Managent;
using APIGateway.Model.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIGateway.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ServicesController : ControllerBase
	{
		ILogger<ServicesController> _logger;

		LoadBalancer<BrazoService> _brazoService;

		public ServicesController(ILogger<ServicesController> logger, LoadBalancer<BrazoService> brazoServices)
		{
			_logger = logger;
			_brazoService = brazoServices;
		}

		[HttpGet]
		[Route("services")]
		public async Task<IList<APIServiceStatus>> GetServices()
		{
			return await _brazoService.GetStatusServices();
		}

		[HttpGet]
		[Route("services/running")]
		public async Task<dynamic> GetRunningService()
		{
			return (await _brazoService.GetRunningService()).GetName();
		}

		/*
		[HttpGet]
		[Route("services/configuration")]
		public async Task<dynamic> GetRunningService()
		{
			
			return (await _brazoService.GetRunningService()).GetName();
		}
		*/

	}
}
