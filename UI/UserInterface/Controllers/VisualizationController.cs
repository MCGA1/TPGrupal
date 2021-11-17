using Microsoft.AspNetCore.Mvc;

namespace UserInterface.Controllers
{
	public class VisualizationController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
