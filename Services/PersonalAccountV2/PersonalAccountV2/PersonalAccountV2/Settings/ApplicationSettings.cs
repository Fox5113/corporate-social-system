﻿namespace PersonalAccountV2.Settings
{
    public class ApplicationSettings
    {
        public string ConnectionString { get; set; }
        public string SiteUrl { get; set; }
        public string RabbitMqQueue { get; set; }
        public string HostName { get; set; }

        public ApplicationSettings()
        {
            ConnectionString = "Server=localhost;Database=PersonalAccount;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true;encrypt=false;";
            SiteUrl = "https://localhost:5201";
            HostName = "localhost";
            RabbitMqQueue = "MyQueue";
        }
    }
}
