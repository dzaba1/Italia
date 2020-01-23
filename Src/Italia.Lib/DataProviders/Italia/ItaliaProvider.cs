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
            var resp = await http.GetStringAsync(settings.Url);
            var json = JObject.Parse(resp);

            return TransformJson(json).ToArray();
        }

        private IEnumerable<Offer> TransformJson(JObject json)
        {
            return json["data"].Select(j => TransformOffer((JObject)j));
        }

        private Offer TransformOffer(JObject json)
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
                Url = (string)json["url"]
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
