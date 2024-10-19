using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebScraping.Models
{
    public class Article
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)] // Otomatik artan ObjectId kullanmak için
        public string id { get; set; }

        [BsonElement("yayin_adi")]
        public string yayin_adi { get; set; }

        [BsonElement("yazar_adi")]
        public string yazar_adi { get; set; }

        [BsonElement("yayin_turu")]
        public string yayin_turu { get; set; }

        [BsonElement("yayinlanma_tarihi")]
        public DateTime yayinlanma_tarihi { get; set; }

        [BsonElement("konular")]
        public string konular { get; set; }

        [BsonElement("anahtarkelime_motor")]
        public string anahtarkelime_motor { get; set; }

        [BsonElement("anahtarkelime_makale")]
        public string anahtarkelime_makale { get; set; }

        [BsonElement("ozet")]
        public string ozet { get; set; }

        [BsonElement("referanslar")]
        public string referanslar { get; set; }

        [BsonElement("alinti_sayisi")]
        public int alinti_sayisi { get; set; }

        [BsonElement("doi_numarasi")]
        public string doi_numarasi { get; set; }

        [BsonElement("url_adres")]
        public string url_adres { get; set; }

        [BsonElement("download")]
        public string download { get; set; }
    }
}
