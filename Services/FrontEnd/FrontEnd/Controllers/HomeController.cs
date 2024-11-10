using FrontEnd.Models;
using FrontEnd.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FrontEnd.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NewsService _newsService;

        public HomeController(ILogger<HomeController> logger, NewsService newsService)
        {
            _logger = logger;
            _newsService = newsService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var newsList = await _newsService.GetPublishedListAsync(1, 10, new Guid("35044f8e-065d-4b59-9d4c-f393ea8a90b6"));
                return View(newsList);
            }
            catch (Exception ex)
            {
                return View(new List<NewsViewModel>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Like(string newsId)
        {
            if (!string.IsNullOrEmpty(newsId) && Guid.TryParse(newsId, out var id)) {

                var likesInfo = await _newsService.Like(id, new Guid("35044f8e-065d-4b59-9d4c-f393ea8a90b6"));
                return Ok(likesInfo);
            }

            return Ok();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Timesheet()
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
