namespace WebScraping.Models
{
    public class WebScrapingDatabaseSettings : IWebScrapingDatabaseSettings
    {
        public string WebScrapingCollectionName { get; set; } = String.Empty;
        public string ConnectionString { get; set; } = String.Empty;
        public string DatabaseName { get; set; } = String.Empty;
    }
}
