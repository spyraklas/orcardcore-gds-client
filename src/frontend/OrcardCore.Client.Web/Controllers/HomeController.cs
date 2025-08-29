using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OrcardCore.Client.Web.Models;
using OrcardCore.Client.Web.Repositories;

namespace OrcardCore.Client.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPageRepository _pageRepository;

        public HomeController(ILogger<HomeController> logger, IPageRepository pageRepository)
        {
            _logger = logger;
            _pageRepository = pageRepository;
        }

        public async Task<IActionResult> Index()
        {
            var pages = await _pageRepository.GetPages();

            if (pages != null && pages.Count > 0 && pages.Where(x => x.FileName.ToLower() == "home").FirstOrDefault() != null)
            {
                var model = new PageModel()
                {
                    PageInfo = pages.Where(x => x.FileName.ToLower() == "home").FirstOrDefault()
                };

                return View(model);
            }
            else
            {
                _logger.LogInformation("No pages found.");
                return RedirectToAction("Error");
            }

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
