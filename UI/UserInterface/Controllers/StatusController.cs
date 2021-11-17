using Microsoft.AspNetCore.Mvc;

namespace UserInterface.Controllers
{
	public class StatusController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
