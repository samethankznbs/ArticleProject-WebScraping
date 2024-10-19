using MongoDB.Bson;
using MongoDB.Driver;
using System.Globalization;
using System.Reflection.Metadata;
using WebScraping.Models;

namespace WebScraping.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IMongoCollection<Article> _article;

        public ArticleService(IWebScrapingDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _article = database.GetCollection<Article>(settings.WebScrapingCollectionName);
        }

        public Article Create(Article article)
        {
            _article.InsertOne(article);
            return article;
        }

        public List<Article> Get()
        {
            return _article.Find(article => true).ToList();
        }

        public Article Get(string id)
        {
            return _article.Find(article => article.id == id).FirstOrDefault();
        }

        public void Remove(string id)
        {
            _article.DeleteOne(article => article.id == id);
        }

        public void Update(string id, Article article)
        {
            _article.ReplaceOne(article => article.id == id, article);
        }

        public List<Article> GetAllList()
        {
            return _article.Find(article => true).ToList();

        }
        public List<Article> SortByPublisher(List<Article> articles)
        {
            // Türkçe karakterlerin doğru sıralanması için özel bir kural belirleyebiliriz
            var sortedArticles = articles.OrderBy(a => a.yayin_adi, StringComparer.Create(new CultureInfo("tr-TR"), false)).ToList();
            return sortedArticles;
        }

        public List<Article> SortByPublishDateNew(List<Article> articles)
        {
            // Yayınlanma tarihine göre artan şekilde sıralama yap
            var sortedArticles = articles.OrderBy(a => a.yayinlanma_tarihi).ToList();
            return sortedArticles;
        }

        public List<Article> SortByPublishDateOld(List<Article> articles)
        {
            // Yayınlanma tarihine göre azalan şekilde sıralama yap
            var sortedArticles = articles.OrderByDescending(a => a.yayinlanma_tarihi).ToList();
            return sortedArticles;
        }

        public List<Article> SortByReferenceCountBig(List<Article> articles)
        {
            // Atıf sayısına göre azalan şekilde sıralama yap
            var sortedArticles = articles.OrderByDescending(a => a.alinti_sayisi).ToList();
            return sortedArticles;
        }

        public List<Article> SortByReferenceCountSmall(List<Article> articles)
        {
            // Atıf sayısına göre artan şekilde sıralama yap
            var sortedArticles = articles.OrderBy(a => a.alinti_sayisi).ToList();
            return sortedArticles;
        }

        public List<Article> SearchByYayinAdi(string search_yayin_adi)
        {
            // Verilen yayin adını içeren makaleleri filtrele
            var filter = Builders<Article>.Filter.Regex(a => a.yayin_adi, new BsonRegularExpression(search_yayin_adi, "i")); // "i" parametresi büyük/küçük harf duyarlılığını kapatır

            // Filtrelenmiş makaleleri getir
            var searchedArticles = _article.Find(filter).ToList();

            return searchedArticles;
        }
        public List<Article> SearchByYazarAdi(string search_yazar_adi)
        {
            // Verilen yayin adını içeren makaleleri filtrele
            var filter = Builders<Article>.Filter.Regex(a => a.yazar_adi, new BsonRegularExpression(search_yazar_adi, "i")); // "i" parametresi büyük/küçük harf duyarlılığını kapatır

            // Filtrelenmiş makaleleri getir
            var searchedArticles = _article.Find(filter).ToList();

            return searchedArticles;
        }
        public List<Article> SearchByOzet(string search_ozet)
        {
            // Verilen yayin adını içeren makaleleri filtrele
            var filter = Builders<Article>.Filter.Regex(a => a.ozet, new BsonRegularExpression(search_ozet, "i")); // "i" parametresi büyük/küçük harf duyarlılığını kapatır

            // Filtrelenmiş makaleleri getir
            var searchedArticles = _article.Find(filter).ToList();

            return searchedArticles;
        }
        public List<Article> SearchByKeywords(string search_keywords)
        {
            // Verilen yayin adını içeren makaleleri filtrele
            var filter = Builders<Article>.Filter.Regex(a => a.anahtarkelime_makale, new BsonRegularExpression(search_keywords, "i")); // "i" parametresi büyük/küçük harf duyarlılığını kapatır

            // Filtrelenmiş makaleleri getir
            var searchedArticles = _article.Find(filter).ToList();

            return searchedArticles;
        }
        public List<Article> SearchById(string search_id)
        {
            // Verilen yayin adını içeren makaleleri filtrele
            var filter = Builders<Article>.Filter.Regex(a => a.id, new BsonRegularExpression(search_id, "i")); // "i" parametresi büyük/küçük harf duyarlılığını kapatır

            // Filtrelenmiş makaleleri getir
            var searchedArticles = _article.Find(filter).ToList();

            return searchedArticles;
        }

    }
}
