using System.Collections.Generic;
using System.Linq;

namespace QuoteSaver
{
    public class Quote
    {
        public Quote(string quote, string author, int likes, List<string> tags, int pk, string image, string language)
        {
            QuoteText = quote;
            Author = author;
            Likes = likes;
            Tags = tags.ToList();
            Pk = pk;
            Image = image;
            Language = language;

        }

        public string Language { get; set; }
        public string Image { get; set; }

        public int Pk { get; set; }

        public List<string> Tags { get; set; }

        public int Likes { get; set; }

        public string Author { get; set; }

        public string QuoteText { get; set; }
    }
}