﻿using Microsoft.AspNetCore.Mvc;

namespace UserInterface.Controllers
{
	public class ReportController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
