using FrontEnd.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            if (User.Identity != default && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

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
                    HttpContext.Response.Cookies.Append("jwtToken", token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        Expires = DateTime.UtcNow.AddMinutes(30)
                    });

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, username) // или другие необходимые данные
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.ErrorMessage = "Неправильные учетные данные!";
                    return View("~/Views/Login/Login.cshtml");
                }
            }
            catch (HttpRequestException)
            {
                ViewBag.ErrorMessage = "Сервис авторизации недоступен, приносим свои извинения.";
                return View("~/Views/Login/Login.cshtml");
            }
        }
    }
}
