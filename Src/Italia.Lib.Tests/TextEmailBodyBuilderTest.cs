using AutoFixture;
using Dzaba.TestUtils;
using FluentAssertions;
using Italia.Lib.Notifications.Email;
using NUnit.Framework;

namespace Italia.Lib.Tests
{
    [TestFixture]
    public class TextEmailBodyBuilderTest
    {
        private IFixture fixture;

        [SetUp]
        public void Setup()
        {
            fixture = TestFixture.Create();
        }

        private TextEmailBodyBuilder CreateSut()
        {
            return fixture.Create<TextEmailBodyBuilder>();
        }

        [Test]
        public void Generate_WhenOffersProvided_ThanItMakesAText()
        {
            var offers = new OffersToNotify();
            offers.AddNewOffer(TestData.BuildOffer("111", true, 1000, "Hotel1", "Country", "Url1", "Departure", "DataProvider"));
            offers.AddOfferIsGone(TestData.BuildOffer("112", true, 900, "Hotel2", "Country", "Url2", "Departure", "DataProvider"));
            offers.AddOfferChanged(TestData.BuildOffer("113", true, 1100, "Hotel3", "Country", "Url3", "Departure", "DataProvider"),
                TestData.BuildOffer("113", true, 1200, "Hotel3", "Country", "Url3", "Departure", "DataProvider"));
            offers.AddOfferActiveAgain(TestData.BuildOffer("114", true, 1300, "Hotel4", "Country", "Url4", "Departure", "DataProvider"),
                TestData.BuildOffer("114", true, 1300, "Hotel4", "Country", "Url4", "Departure", "DataProvider"));

            var sut = CreateSut();

            var result = sut.Generate(offers);

            result.Body.Should().NotBeNullOrWhiteSpace();
            result.IsHtml.Should().BeFalse();
        }
    }
}
