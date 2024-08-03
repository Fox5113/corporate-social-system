namespace WebApi.Settings
{
    /// <summary>
    /// Настройки приложения
    /// </summary>
    public class ApplicationSettings
    {
        public string ConnectionString { get; set; }

        public ApplicationSettings() {
            ConnectionString = "Server=localhost;Database=NewsFeed;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true;encrypt=false;";
        }
    }
}
