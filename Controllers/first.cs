using Microsoft.AspNetCore.Mvc;
using HtmlAgilityPack;
using System;
using System.Net.Http;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using WebScraping.Models;
using WebScraping.Services;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Net;
using System.Diagnostics;

namespace WebScraping.Controllers
{
    public static class LinkHelper
    {
        public static List<string> Links { get; set; } = new List<string>();
    }
    public class first : Controller
    {
        static List<string> links = LinkHelper.Links;
        private readonly IArticleService ArticleService;
        static string arama = "";
        static List<Article> articles = new List<Article>();
        int count;
        int now_count;
        public first(IArticleService ArticleService)
        {
            this.ArticleService = ArticleService;
        }



        public IActionResult Index(String search)
        {
            arama = search;

            String url = "https://dergipark.org.tr/tr/search?q=" + search;
            url = url + "&section=articles";
            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url).Result;
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);



            HtmlNodeCollection articleCardNodes = htmlDocument.DocumentNode.SelectNodes("//div[contains(@class, 'card-body')]");
            HtmlNodeCollection linkNodes = htmlDocument.DocumentNode.SelectNodes("//h5[@class='card-title']/a");

            // Her bir a etiketinin href özniteliğine eriş

            foreach (HtmlNode linkNode in linkNodes)
            {
                string hrefValue = linkNode.GetAttributeValue("href", "");
                links.Add(hrefValue);
            }
            // articleCardNodes içindeki tüm elementlerin içeriğine eriş
            List<string> articleCardContents = new List<string>();

            foreach (HtmlNode articleCardNode in articleCardNodes)
            {
                string innerHtml = articleCardNode.InnerHtml; // İçerik
                articleCardContents.Add(innerHtml);
            }

            // View'e verileri aktar
            ViewBag.ArticleCardContents = articleCardContents;


            




            return View();
        }
        public IActionResult Index1(int id)
        {







            id = id - 1;

            String url = links[id];
            links.Clear();
            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url).Result;
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            HtmlNodeCollection articleCardNodes = htmlDocument.DocumentNode.SelectNodes("//div[contains(@class, 'col-lg-9 col-md-8')]");
            List<string> articleCardContents = new List<string>();
            foreach (HtmlNode articleCardNode in articleCardNodes)
            {
                string innerHtml = articleCardNode.InnerHtml; // İçerik
                articleCardContents.Add(innerHtml);
            }
            ViewBag.ArticleCardContents = articleCardContents;

            HtmlNode node = htmlDocument.DocumentNode.SelectSingleNode("//h3[@class='article-title']");
            string title = "";
            if (node != null)
            {
                title = node.InnerText.Trim(); // Başlığı alın ve boşlukları temizleyin
                //Console.WriteLine("Başlık: " + title); // Başlığı consola yazdırın
            }
            else
            {
               // Console.WriteLine("Başlık düğümü bulunamadı.");
            }
            node = htmlDocument.DocumentNode.SelectSingleNode("//p[@class='article-authors']");

            string authorName = "";
            if (node != null)
            {
                authorName = node.InnerText.Trim(); // Metin içeriğini alın ve boşlukları temizleyin
                //Console.WriteLine("Yazar Adı: " + authorName); // Adı consola yazdırın
            }
            else
            {
                //Console.WriteLine("Yazar adı bulunamadı.");
            }
            node = htmlDocument.DocumentNode.SelectSingleNode("//th[text()='Bölüm']/following-sibling::td");

            string type = "";
            if (node != null)
            {
                type = node.InnerText.Trim(); // Metin içeriğini alın ve boşlukları temizleyin
               // Console.WriteLine("Yayın Türü: " + type); // Adı consola yazdırın
            }
            else
            {
                //Console.WriteLine("Yayın Türü bulunamadı.");
            }
            node = htmlDocument.DocumentNode.SelectSingleNode("//th[text()='Yayımlanma Tarihi']/following-sibling::td");

            string date = "";
            if (node != null)
            {
                date = node.InnerText.Trim(); // Metin içeriğini alın ve boşlukları temizleyin
               // Console.WriteLine("Yayımlanma Tarihi : " + date); // Adı consola yazdırın
            }
            else
            {
                //Console.WriteLine("Yayınlanma Tarihi bulunamadı.");
            }
            node = htmlDocument.DocumentNode.SelectSingleNode("//th[text()='Konular']/following-sibling::td");

            string subject = "";
            if (node != null)
            {
                subject = node.InnerText.Trim(); // Metin içeriğini alın ve boşlukları temizleyin
               // Console.WriteLine("Konu : " + subject); // Adı consola yazdırın
            }
            else
            {
                //Console.WriteLine("Konu bulunamadı.");
            }
            node = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='article-keywords data-section']");

            string keywords_artcle = "";
            if (node != null)
            {
                // Anahtar kelimelerin bulunduğu <a> etiketlerini seçin
                HtmlNodeCollection keywordNodes = node.SelectNodes(".//a");
                if (keywordNodes != null)
                {
                    // Anahtar kelimeleri bir string olarak birleştirin
                    keywords_artcle = string.Join(", ", keywordNodes.Select(node => node.InnerText.Trim()));
                    //Console.WriteLine("Anahtar Kelimeler: " + keywords_artcle); // Anahtar kelimeleri consola yazdırın
                }
                else
                {
                    //Console.WriteLine("Anahtar kelimeler bulunamadı.");
                }
            }
            else
            {
               // Console.WriteLine("Anahtar kelimeler düğümü bulunamadı.");
            }
            node = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='article-abstract data-section']");

            string summary = "";
            if (node != null)
            {
                var paragraphs = node.SelectNodes("p");
                if (paragraphs != null)
                {
                    foreach (var paragraph in paragraphs)
                    {
                        summary = paragraph.InnerText;
                       // Console.WriteLine("Paragraf İçeriği: " + summary);
                    }
                }
                else
                {
                    //Console.WriteLine("Bu div elementinin içinde <p> elementi bulunamadı.");
                }
            }
            else
            {
               // Console.WriteLine("Belirtilen sınıfa sahip div elementi bulunamadı.");
            }
            HtmlNodeCollection rows = htmlDocument.DocumentNode.SelectNodes("//table[@class='table table-striped m-table cite-table']/tbody/tr");
            int atif_Sayi = 0;
            if (rows != null)
            {
                // Dergi adlarını birleştireceğimiz boş bir string oluşturun
                foreach (HtmlNode row in rows)
                {
                    // Her satırda dergi adı içeren ikinci sütunu seçin
                    HtmlNode dergiNode = row.SelectSingleNode("./td[2]");
                    if (dergiNode != null)
                    {
                        atif_Sayi++;
                    }
                }
                // Son virgülü kaldırın

               // Console.WriteLine("Atıf Sayisi: " + atif_Sayi); // Birleştirilen dergi adlarını consola yazdırın
            }
            else
            {
               // Console.WriteLine("Atıf 0");
            }
            node = htmlDocument.DocumentNode.SelectSingleNode("//a[@class='doi-link']");

            string doi = "";
            if (node != null)
            {
                doi = node.InnerText.Trim(); // Metin içeriğini alın ve boşlukları temizleyin
               // Console.WriteLine("Doi : " + doi); // Adı consola yazdırın
            }
            else
            {
               // Console.WriteLine("Konu bulunamadı.");
            }
            node = htmlDocument.DocumentNode.SelectSingleNode("//a[contains(@class, 'btn') and contains(@class, 'btn-sm') and contains(@class, 'article-tool') and contains(@class, 'pdf')]");
            string download = node?.Attributes["href"]?.Value;
            download = "https://dergipark.org.tr" + download;
            

            if (node != null)
            {


                //Console.WriteLine("DOWNLOAD SLKDFGHJSLDHGLKSDHGLKSDHG : " + download); // Adı consola yazdırın
            }
            else
            {
               // Console.WriteLine("Konu bulunamadı.");
            }



            var articleCitations = htmlDocument.DocumentNode.SelectSingleNode("//div[contains(@class, 'article-citations') and contains(@class, 'data-section')]");
            var listItems = articleCitations?.SelectNodes(".//ul[contains(@class, 'fa-ul')]/li");
            string concatenatedString = "";
            if (listItems != null && listItems.Count > 0)
            {
                // String oluştur

                foreach (var item in listItems)
                {
                    concatenatedString += item.InnerText.Trim() + ", ";
                }

                // Son virgülü kaldır
                concatenatedString = concatenatedString.TrimEnd(' ', ',');

                //Console.WriteLine(concatenatedString);
            }
            else
            {
                //Console.WriteLine("No list items found.");
            }
            //Console.WriteLine("--------------------------------------------------------------------------------------------------------------------------------");


            CultureInfo culture = new CultureInfo("tr-TR");

            // Tarih stringini belirli bir formata göre çözümle
            DateTime date_;
            if (DateTime.TryParseExact(date, "d MMMM yyyy", culture, DateTimeStyles.None, out date_))
            {
                //Console.WriteLine($"Original String: {date}");
                //Console.WriteLine($"Parsed Date: {date_:dd/MM/yyyy}"); // İstediğiniz formata göre yazdırabilirsiniz
            }
            else
            {
                Console.WriteLine($"Could not parse '{date}' to a valid date.");
            }
            string stringWithoutSpaces = authorName.Replace(" ", "");
            //Console.WriteLine("Yayin adi :" + title);
            //Console.WriteLine("Yazar adi :" + stringWithoutSpaces);
            //Console.WriteLine("Yayin turu :" + type);
            //Console.WriteLine("Yayimlanma tarihi:" + date);
            //Console.WriteLine("Konular :" + subject);
            //Console.WriteLine("Aramada aratılan : :" + arama);
            //Console.WriteLine("Anahtar kelimeler :" + keywords_artcle);
            //Console.WriteLine("Özet :" + summary);
            //Console.WriteLine("Referanslar :" + concatenatedString);
            //Console.WriteLine("Atıf Sayısı :" + atif_Sayi);
            //Console.WriteLine("Doi Numarası :" + doi);
            //Console.WriteLine("Url Adresi :" + url);



            if(title.Length != 0 && summary.Length != 0 ) {
                Article article = new Article
                {

                    yayin_adi = title,
                    yazar_adi = stringWithoutSpaces,
                    yayin_turu = type,
                    yayinlanma_tarihi = date_,
                    konular = subject,
                    anahtarkelime_motor = arama,
                    anahtarkelime_makale = keywords_artcle,
                    ozet = summary,
                    referanslar = concatenatedString,
                    alinti_sayisi = atif_Sayi,
                    doi_numarasi = doi,
                    url_adres = url,
                    download = download


                };



                ArticleService.Create(article);
                title = title + ".pdf";
                string savePath = @"C:\Users\samethan\Desktop\pdfler\" + title;

                DownloadFile(download, savePath);

            }
            else
            {
                Console.WriteLine("Veri Çekilemedi. Pdf indirilemedi");
            }



            return View();
        }
        public IActionResult Index2(int id,String search_yayin_adi,String search_yazar_adi, String search_ozet , String search_keywords , String search_id)
        {
            
            if (id==0 && search_yayin_adi== null && search_yazar_adi == null && search_ozet == null && search_keywords == null && search_id == null)
            {
                articles = ArticleService.GetAllList();
                count=articles.Count;
                
            }
            if (id==1)
            {
                articles = ArticleService.SortByPublisher(articles);
                
            }
            if (id == 2)
            {   
                articles = ArticleService.SortByPublishDateNew(articles);
                
            }
            if (id == 3)
            {
                articles = ArticleService.SortByPublishDateOld(articles);
                
            }
            if (id == 4)
            {
                articles = ArticleService.SortByReferenceCountBig(articles);
                
            }
            if (id == 5)
            {
                articles = ArticleService.SortByReferenceCountSmall(articles);
            
            }
           
            if (id== 6)
            {
                if(search_yayin_adi!=null)
                {
                    articles= ArticleService.SearchByYayinAdi(search_yayin_adi);
                }
                if (search_yazar_adi != null)
                {
                    articles = ArticleService.SearchByYazarAdi(search_yazar_adi);
                }
                if (search_ozet != null)
                {
                    articles = ArticleService.SearchByOzet(search_ozet);
                }
                if (search_keywords != null)
                {
                    articles = ArticleService.SearchByKeywords(search_keywords);
                }
                if (search_id != null)
                {
                    articles = ArticleService.SearchById(search_id);
                }


            }





            ViewBag.Articles = articles;


            return View();
        }

        static void DownloadFile(string url, string savePath)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(url, savePath);
                    Console.WriteLine("PDF başarıyla indirildi ve kaydedildi.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
            }
        }
        public IActionResult Index3(string url)
        {


            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url).Result;
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            HtmlNodeCollection articleCardNodes = htmlDocument.DocumentNode.SelectNodes("//div[contains(@class, 'tab-content')]");
            List<string> articleCardContents = new List<string>();
            foreach (HtmlNode articleCardNode in articleCardNodes)
            {
                string innerHtml = articleCardNode.InnerHtml; // İçerik
                articleCardContents.Add(innerHtml);
            }
            ViewBag.ArticleCardContents = articleCardContents;









            return View();

        }
        public IActionResult ShowPdf(string url)
        {
            Console.WriteLine(url);
            WebClient webClient = new WebClient();
            byte[] pdfBytes = webClient.DownloadData(url);

            // İndirilen PDF dosyasını MemoryStream'e yükle
            MemoryStream pdfStream = new MemoryStream(pdfBytes);

            // MemoryStream'deki PDF dosyasını döndür
            return new FileStreamResult(pdfStream, "application/pdf");
        }



    }

}