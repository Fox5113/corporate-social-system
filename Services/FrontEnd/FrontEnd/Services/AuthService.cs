using FrontEnd.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace FrontEnd.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserViewModel> AuthenticateAsync(string username, string password)
        {
            var requestBody = new { username, password };
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7272/api/Authentication/login", requestBody);

            if (response.IsSuccessStatusCode)
            {
                var user = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                return JsonSerializer.Deserialize<UserViewModel>(user, options);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return null;
            }
            else
            {
                throw new HttpRequestException("Authorization service is not available.");
            }
        }

        public async Task<UserViewModel> GetUser(string username)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7272/api/Users/GetUserByName?name={username}");

            if (response.IsSuccessStatusCode)
            {
                var user = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                return JsonSerializer.Deserialize<UserViewModel>(user, options);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return null;
            }
            else
            {
                throw new HttpRequestException("Authorization service is not available.");
            }
        }

        public async void LogoutAsync(string id)
        {
            var requestBody = new { id };

            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7272/api/Authentication/logout", requestBody);

            if (response.IsSuccessStatusCode)
            {
                return;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new HttpRequestException("The user is not authorized.");
            }
            else
            {
                throw new HttpRequestException("Authorization service is not available.");
            }
        }
    }
}
