using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using UserInterface.Models.DTO;

namespace UserInterface.Controllers
{
	public class VisualizationController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public ActionResult ReadData()
		{

			return Json(new
			{
				ingresados = GetFooData(),
				enproceso = GetFooData(),
				apilados = GetFooData()
			});
		}

		public IList<PackageItem> GetFooData()
		{ 
			Random r = new Random();
			var result = new List<PackageItem>();
			var count = r.Next(1, 40);
			for (int i = 0; i < count; i++)
			{
				result.Add(new PackageItem() {  CreationDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, r.Next(24), r.Next(60), r.Next(60)) });
			}

			return result;
		}
	}
}
