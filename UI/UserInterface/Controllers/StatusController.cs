using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserInterface.Controllers
{
	public class StatusController : Controller
	{
		private APIGatewayService _service;
		private ILogger<StatusController> _logger;

		public StatusController(ILogger<StatusController> logger, APIGatewayService service)
		{
			_service = service;
			_logger = logger;
		}

		public async Task<IActionResult> Index()
		{
			try
			{
				_logger.LogInformation("Getting status info");

				List<(ServiceType, ICollection<APIServiceStatus>)> result = new();

				result.Add((ServiceType.Brazo, await _service.ServicesAllAsync(ServiceType.Brazo)));
				result.Add((ServiceType.Cinta, await _service.ServicesAllAsync(ServiceType.Cinta)));
				result.Add((ServiceType.Prensa, await _service.ServicesAllAsync(ServiceType.Prensa)));

				return View(result);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Error processing view Get Status");
				throw;
			}
			
		}

	}
}
