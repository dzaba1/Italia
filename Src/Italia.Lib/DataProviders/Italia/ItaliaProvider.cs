using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dzaba.Utils;
using Italia.Lib.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Italia.Lib.DataProviders.Italia
{
    internal sealed class ItaliaProvider : IDataProvider, IDisposable
    {
        private static readonly Regex CountryRegex = new Regex(@"<a href=""\S+"">(?<Location>\w+)<\/a>", RegexOptions.IgnoreCase);
        private readonly IItaliaSettings settings;
        private readonly HttpClient http;

        public ItaliaProvider(IItaliaSettings settings)
        {
            Require.NotNull(settings, nameof(settings));

            this.settings = settings;
            http = new HttpClient();
        }

        public void Dispose()
        {
            http?.Dispose();
        }

        public async Task<Offer[]> GetOffersAsync()
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, settings.Url))
            {
                using (var resp = await http.SendAsync(request))
                {
                    resp.EnsureSuccessStatusCode();

                    using (var stream = await resp.Content.ReadAsStreamAsync())
                    {
                        using (var textReader = new StreamReader(stream))
                        {
                            using (var jreader = new JsonTextReader(textReader))
                            {
                                var json = JObject.Load(jreader);
                                return TransformJson(json).ToArray();
                            }
                        }
                    }
                }
            }
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
            return CountryRegex.Matches(value)[0].Groups["Location"].Value;
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
