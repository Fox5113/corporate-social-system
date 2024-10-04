namespace WebApi.Settings
{
    /// <summary>
    /// Настройки приложения
    /// </summary>
    public class ApplicationSettings
    {
        public string ConnectionString { get; set; }
        public string SiteUrl { get; set; }
        public string RabbitMqQueue { get; set; }
        public string HostName { get; set; }

        public ApplicationSettings() {
            //SiteUrl = "https://localhost:5201";
            HostName = "localhost";
            RabbitMqQueue = "MyQueue";
        }
    }
}
