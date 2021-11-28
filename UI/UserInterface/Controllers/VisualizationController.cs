using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserInterface.Models.DTO;

namespace UserInterface.Controllers
{
	public class VisualizationController : Controller
	{
		private APIGatewayService _service;
		private ILogger<VisualizationController> _logger;

		public VisualizationController(ILogger<VisualizationController> logger, APIGatewayService service)
		{
			_service = service;
			_logger = logger;
		}

		public IActionResult Index()
		{
			return View();
		}

		public async Task<ActionResult> ReadData()
		{
			_logger.LogInformation("Reading data for visualization");
			
			
			return Json(new
			{
				ingresados = await GetItemsByType(ServiceType.Cinta),
				enproceso = await GetItemsByType(ServiceType.Brazo),
				apilados = await GetItemsByType(ServiceType.Prensa)
			});
		}

		public async Task<IList<PackageItem>> GetItemsByType(ServiceType type)
		{
			try
			{
				_logger.LogInformation($"Getting data by type [{type}]");

				return (await _service.PackageAsync(type)).ToList();
			}
			catch (Exception e)
			{
				_logger.LogError(e, $"Error getting data from service type [{type}]");

				return new List<PackageItem>();
			}
		}
	}
}
