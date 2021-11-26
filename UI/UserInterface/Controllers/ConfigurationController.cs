using Microsoft.AspNetCore.Mvc;

namespace UserInterface.Controllers
{
	public class ConfigurationController : Controller
	{
		private APIGatewayService _service;

		public ConfigurationController(APIGatewayService service)
		{
			_service = service;
		}

		public IActionResult Index()
		{
			return View();
		}
	}
}
