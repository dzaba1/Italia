using System;
using System.Collections.Generic;
using Italia.Lib.Model;

namespace Italia.Lib.Tests
{
    internal static class TestData
    {
        public static IEnumerable<Offer> PollSomeOffers()
        {
            yield return new Offer
            {
                ExternalReference = "111",
                Created = DateTime.Now,
                Active = true,
                Modified = DateTime.Now,
                Price = 1000,
                From = DateTime.Now,
                To = DateTime.Now.AddDays(7),
                HotelName = "Hotel1",
                Country = "Country1",
                Url = "Url",
                Departure = "Departure",
                DataProvider = "DataProvider1"
            };

            yield return new Offer
            {
                ExternalReference = "112",
                Created = DateTime.Now,
                Active = true,
                Modified = DateTime.Now,
                Price = 1100,
                From = DateTime.Now,
                To = DateTime.Now.AddDays(7),
                HotelName = "Hotel2",
                Country = "Country1",
                Url = "Url",
                Departure = "Departure",
                DataProvider = "DataProvider1"
            };

            yield return new Offer
            {
                ExternalReference = "111",
                Created = DateTime.Now,
                Active = true,
                Modified = DateTime.Now,
                Price = 1000,
                From = DateTime.Now,
                To = DateTime.Now.AddDays(7),
                HotelName = "Hotel1",
                Country = "Country1",
                Url = "Url",
                Departure = "Departure",
                DataProvider = "DataProvider2"
            };
        }
    }
}
