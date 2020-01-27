using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dzaba.Utils;
using Italia.Lib.Model;
using Newtonsoft.Json.Linq;

namespace Italia.Lib.DataProviders.Italia
{
    internal sealed class ItaliaProvider : IDataProvider
    {
        private static readonly Regex CountryRegex = new Regex(@"<a href=""\S+"">(?<Location>[\w ]+)<\/a>", RegexOptions.IgnoreCase);
        private readonly IItaliaSettings settings;
        private readonly IHttp http;

        public ItaliaProvider(IItaliaSettings settings,
            IHttp http)
        {
            Require.NotNull(settings, nameof(settings));
            Require.NotNull(http, nameof(http));

            this.settings = settings;
            this.http = http;
        }

        public async Task<Offer[]> GetOffersAsync()
        {
            var offers = new HashSet<Offer>(OfferExternalReferenceComparer.Instance);

            var tasks = new List<Task<Offer[]>>(settings.Urls.Length);
            foreach (var url in settings.Urls)
            {
                tasks.Add(GetOffersAsync(url));
            }

            foreach (var task in tasks)
            {
                offers.AddRange(await task);
            }

            return offers.ToArray();
        }

        private async Task<Offer[]> GetOffersAsync(Uri url)
        {
            var page = 1;
            var offers = new List<Offer>();
            while (true)
            {
                var pageOffers = await GetPageAsync(url, page);
                if (pageOffers.Any())
                {
                    offers.AddRange(pageOffers);
                    page++;
                }
                else
                {
                    break;
                }
            }

            return offers.ToArray();
        }

        private async Task<Offer[]> GetPageAsync(Uri url, int page)
        {
            var urlPaged = new Uri(url + $"&page={page}");
            var resp = await http.GetStringAsync(urlPaged);
            var json = JObject.Parse(resp);

            return TransformJson(json, url).ToArray();
        }

        private IEnumerable<Offer> TransformJson(JObject json, Uri url)
        {
            return json["data"].Select(j => TransformOffer((JObject)j, url));
        }

        private Offer TransformOffer(JObject json, Uri url)
        {
            return new Offer
            {
                Active = true,
                Country = GetCountry((string)json["canonicalDestinationTitle"]),
                DataProvider = "Italia",
                Departure = (string)json["departure"]["from"]["city"],
                ExternalReference = (string)json["offerId"][settings.ReferencePropertyName],
                From = GetDate((string)json["dateFrom"]),
                HotelName = (string)json["title"],
                OriginalPrice = GetPrice((int)json["oldPrice"]),
                Price = GetPrice((int)json["price"]),
                To = GetDate((string)json["dateTo"]),
                Url = new Uri(url.GetHostWithScheme(), (string)json["url"])
            };
        }

        private string GetCountry(string value)
        {
            var matches = CountryRegex.Matches(value);
            return matches[0].Groups["Location"].Value;
        }

        private decimal GetPrice(int value)
        {
            return value / (decimal)100;
        }

        private DateTime GetDate(string value)
        {
            return DateTime.ParseExact(value, "yyyy-MM-dd", null);
        }
    }
}
