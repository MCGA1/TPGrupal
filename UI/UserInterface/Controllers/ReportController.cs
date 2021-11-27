using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserInterface.Models.DTO;

namespace UserInterface.Controllers
{
	public class ReportController : Controller
	{
		private APIGatewayService _service;
		public ReportController(APIGatewayService service)
		{
			_service= service;
		}
		public async Task<IActionResult> Index()
		{
			var list = await _service.ReportAsync();
			return View(list);
		}
	}
}
