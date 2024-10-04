namespace FrontEnd.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> AuthenticateAsync(string username, string password)
        {
            var requestBody = new { username, password };
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7192/api/User/login", requestBody);

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                return token;
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
    }
}
