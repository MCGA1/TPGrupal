using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserInterface.Controllers
{
	public class StatusController : Controller
	{
		private APIGatewayService _service;

		public StatusController(APIGatewayService service)
		{
			_service = service;
		}

		public async Task<IActionResult> Index()
		{
			List<(ServiceType, ICollection<APIServiceStatus>)> result = new();

			result.Add((ServiceType.Brazo, await _service.ServicesAllAsync(ServiceType.Brazo)));
			result.Add((ServiceType.Cinta, await _service.ServicesAllAsync(ServiceType.Cinta)));
			result.Add((ServiceType.Prensa, await _service.ServicesAllAsync(ServiceType.Prensa)));
			
			return View(result);
		}

	}
}
