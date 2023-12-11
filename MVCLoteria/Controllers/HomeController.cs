using DatosLoteria.Data;
using Microsoft.AspNetCore.Mvc;
using MVCLoteria.Models;
using System.Diagnostics;

namespace MVCLoteria.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			_logger.LogInformation("Estoy en la p�gina Index ...");

			return View();
		}

		public IActionResult Privacy()
		{
			_logger.LogInformation("Estoy en la p�gina Privacy ...");

			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			_logger.LogInformation("Estoy en la p�gina Error ...");

			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
