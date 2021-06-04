using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.Json;

namespace QuoteSaver
{
    public class PaperQuotesService
    {
        readonly HttpClient _client;

        public PaperQuotesService()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Authorization", "Token "+Constants.ApiToken);
        }

        public async Task<List<Quote>> GetRandomCuratedQuotesAsync()
        {
            var uri = new Uri(string.Format(Constants.QuoteUri + "?&curated=1&order=-likes&language=en&limit=20"));
            var response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var parsedObject = JObject.Parse(content);
                var jsonResult = parsedObject["results"].ToString();
                var items = JsonConvert.DeserializeObject<List<Quote>>(jsonResult);
                return await Task.FromResult(items);
            }
            else
            {
                return await Task.FromResult<List<Quote>>(null);
            }
        }

        public async Task<List<Quote>> GetSearchedQuoteByTagAsync(String[] tags)
        {
            var tagsString = "";
            foreach (var tag in tags)
            {
                tagsString = tagsString + "," + tag;
            }

            var uri = new Uri(string.Format(Constants.QuoteUri +"?tags=" + tagsString + "&curated=1&order=-likes&language=en&limit=20"));
            var response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var parsedObject = JObject.Parse(content);
                var jsonResult = parsedObject["results"].ToString();
                var items = JsonConvert.DeserializeObject<List<Quote>>(jsonResult);
                return await Task.FromResult(items);
            }
            else
            {
                return await Task.FromResult<List<Quote>>(null);
            }
        }
    }
}