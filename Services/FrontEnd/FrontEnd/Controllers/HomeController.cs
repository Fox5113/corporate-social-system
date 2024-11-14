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

        private async Task InitSessionData()
        {
            try
            {
                if(HttpContext?.Request?.Cookies[Constants.UserIdCookieKey] != null)
                {
                    try
                    {
                        var userModel = await _newsService.GetPersonalAccountData(HttpContext.Request.Cookies[Constants.UserIdCookieKey].ToString());
                        ViewData[Constants.PersonalAccountDataKey] = userModel;
                        HttpContext.Session.SetString(User.Identity.Name, userModel.Id.ToString());
                        HttpContext.Session.SetString(User.Identity.Name + Constants.FullNamePrefix, userModel.Firstname + " " + userModel.Surname);
                        HttpContext.Session.SetString(User.Identity.Name + Constants.LanguagePrefix, !String.IsNullOrEmpty(userModel.Language) ? userModel.Language : Constants.LanguageBase);
                    }
                    catch(Exception ex) { }
                }
                
                if (!String.IsNullOrEmpty(User?.Identity?.Name) && String.IsNullOrEmpty(HttpContext.Session.GetString(User.Identity.Name)))
                {
                    var userModel = await _newsService.GetUserByLogin(User.Identity.Name);
                    if (userModel != null)
                    {
                        HttpContext.Session.SetString(User.Identity.Name, userModel.Id.ToString());
                        HttpContext.Session.SetString(User.Identity.Name + Constants.FullNamePrefix, userModel.Name);
                        HttpContext.Session.SetString(User.Identity.Name + Constants.LanguagePrefix, Constants.LanguageBase);
                    }
                }
            }
            catch (Exception ex) { }
        }

        private void InitViewData()
        {
            var lang = HttpContext.Session.GetString(User.Identity.Name + Constants.LanguagePrefix);
            if (!String.IsNullOrEmpty(User?.Identity?.Name) && !String.IsNullOrEmpty(lang))
            {
                ViewData[Constants.CaptionsKey] = Constants.Dictionaries[lang];
            }

            if (ViewData[Constants.CaptionsKey] == null)
            {
                ViewData[Constants.CaptionsKey] = Constants.Dictionaries[Constants.LanguageBase];
            }

            ViewData[Constants.UserFullNameKey] = HttpContext.Session.GetString(User?.Identity?.Name + Constants.FullNamePrefix);
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                if (!String.IsNullOrEmpty(User?.Identity?.Name) && String.IsNullOrEmpty(HttpContext.Session.GetString(User.Identity.Name)))
                {
                    await InitSessionData();
                }
                InitViewData();

                if (Guid.TryParse(HttpContext.Session.GetString(User.Identity.Name), out var userId))
                {
                    try
                    {
                        var newsList = await _newsService.GetPublishedListAsync(1, 10, userId);
                        ViewData[Constants.NewsFeedListViewDataKey] = newsList;
                    }
                    catch (Exception ex) { }
                }
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Like(string newsId)
        {
            try
            {
                if (!string.IsNullOrEmpty(newsId) && Guid.TryParse(newsId, out var id))
                {
                    if (Guid.TryParse(HttpContext.Session.GetString(User.Identity.Name), out var userId))
                    {
                        var likesInfo = await _newsService.Like(id, userId);
                        return Ok(likesInfo);
                    }
                }
            }
            catch (Exception ex) { }

            return Ok();
        }

        public IActionResult Privacy()
        {
            if (!String.IsNullOrEmpty(User?.Identity?.Name) && String.IsNullOrEmpty(HttpContext.Session.GetString(User.Identity.Name)))
            {
                InitSessionData();
            }
            return View();
        }

        public IActionResult Timesheet()
        {
            if (!String.IsNullOrEmpty(User?.Identity?.Name) && String.IsNullOrEmpty(HttpContext.Session.GetString(User.Identity.Name)))
            {
                InitSessionData();
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
