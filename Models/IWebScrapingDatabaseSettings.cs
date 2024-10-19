namespace WebScraping.Models
{
    public interface IWebScrapingDatabaseSettings
    {
        string WebScrapingCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
