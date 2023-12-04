using Microsoft.AspNetCore.Mvc;

namespace MVCLoteria.Controllers
{
    public class HelloWorldController : Controller
	{
        private readonly ILogger<HelloWorldController> _logger;

        public HelloWorldController(ILogger<HelloWorldController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			_logger.LogInformation("Estoy en la página Index del controlador HelloWorldController.");

			return View();
		}

		// GET: /HelloWorld/Bienvenido/ 
		// Requires using System.Text.Encodings.Web;
		public IActionResult Bienvenido(string name, int numTimes = 1)
		{
            _logger.LogInformation("Estoy en la página Bienvenido del controlador HelloWorldController.");
            ViewData["Message"] = "Hello " + name;
            ViewData["NumTimes"] = numTimes;

            return View();
        }
	}
}
