using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;

namespace FrontEnd.Controllers
{
    public class LoginController : Controller
    {
        private readonly AuthService _authService;

        public LoginController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("/Login")]
        public IActionResult Login()
        {
            return View("~/Views/Login/Login.cshtml");
        }

        [HttpPost("/Login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                var token = await _authService.AuthenticateAsync(username, password);

                if (token != null)
                {
                    HttpContext.Response.Cookies.Append("jwtToken", token);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid credentials";
                    return View("~/Views/Login/Login.cshtml");
                }
            }
            catch (HttpRequestException)
            {
                ViewBag.ErrorMessage = "Authorization service is not available.";
                return View("~/Views/Login/Login.cshtml");
            }
        }
    }
}
