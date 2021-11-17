using Microsoft.AspNetCore.Mvc;

namespace UserInterface.Controllers
{
	public class ConfigurationController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
