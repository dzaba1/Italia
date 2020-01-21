using System;
using System.Collections.Generic;
using Italia.Lib.Model;

namespace Italia.Lib.Tests
{
    internal static class TestData
    {
        public static IEnumerable<Offer> PollSomeOffers()
        {
            yield return BuildOffer("111", true, 1000, "Hotel1", "Country1", "Url", "Departure", "DataProvider1");
            yield return BuildOffer("112", true, 1100, "Hotel2", "Country1", "Url", "Departure", "DataProvider1");
            yield return BuildOffer("111", true, 1100, "Hotel1", "Country1", "Url", "Departure", "DataProvider2");
        }

        public static Offer BuildOffer(string externalReference, bool active, decimal price, string hotelName,
            string country, string url, string departure, string dataProvider)
        {
            return new Offer
            {
                ExternalReference = externalReference,
                Created = DateTime.Now,
                Active = active,
                Modified = DateTime.Now,
                Price = price,
                From = DateTime.Now,
                To = DateTime.Now.AddDays(7),
                HotelName = hotelName,
                Country = country,
                Url = url,
                Departure = departure,
                DataProvider = dataProvider
            };
        }
    }
}
