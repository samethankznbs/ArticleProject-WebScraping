using WebScraping.Models;

namespace WebScraping.Services
{
    public interface IArticleService
    {
        List<Article> Get();
        Article Get(string id);
        Article Create(Article article);
        void Update(string id, Article article);
        void Remove(string id);

        List<Article> GetAllList();

        public List<Article> SortByPublisher(List<Article> articles);
        

        public List<Article> SortByPublishDateNew(List<Article> articles);

        public List<Article> SortByPublishDateOld(List<Article> articles);

        public List<Article> SortByReferenceCountBig(List<Article> articles);

        public List<Article> SortByReferenceCountSmall(List<Article> articles);

        public List<Article> SearchByYayinAdi(string search_yayin_adi);

        public List<Article> SearchByYazarAdi(string search_yazar_adi);

        public List<Article> SearchByOzet(string search_ozet);
        public List<Article> SearchByKeywords(string search_keywords);

        public List<Article> SearchById(string search_id);


    }
}
