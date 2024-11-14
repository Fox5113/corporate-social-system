using FrontEnd.Models;
using FrontEnd.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
                if (String.IsNullOrEmpty(HttpContext.Session.GetString(User.Identity.Name)))
                    return RedirectToAction("Index", "Home");
            }

            return View("~/Views/Login/Login.cshtml");
        }

        [HttpPost("/Login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                var user = await _authService.AuthenticateAsync(username, password);

                if (user != null)
                {
                    if (user.IsFound)
                    {
                        HttpContext.Session.SetString(username, user.Id.ToString());
                        HttpContext.Session.SetString(username+Constants.FullNamePrefix, user.Name);
                        HttpContext.Response.Cookies.Append(Constants.UserIdCookieKey, user.Id.ToString(), new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            Expires = DateTime.UtcNow.AddMinutes(30)
                        });
                    }

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, username)
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

        [HttpPost("/Logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = HttpContext.Request.Cookies[Constants.UserIdCookieKey];

            //_authService.LogoutAsync(userId);

            HttpContext.Response.Cookies.Delete(Constants.UserIdCookieKey);

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }
    }
}
