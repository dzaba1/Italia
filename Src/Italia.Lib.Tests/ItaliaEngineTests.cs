using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Dzaba.TestUtils;
using Italia.Lib.Dal;
using Italia.Lib.Model;
using Italia.Lib.Notifications;
using Moq;
using NUnit.Framework;

namespace Italia.Lib.Tests
{
    [TestFixture]
    public class ItaliaEngineTests
    {
        private IFixture fixture;

        [SetUp]
        public void Setup()
        {
            fixture = TestFixture.Create();
        }

        private ItaliaEngine CreateSut()
        {
            return fixture.Create<ItaliaEngine>();
        }

        private void MockPoll(params Offer[] offers)
        {
            var provider = fixture.FreezeMock<IDataProvider>();
            provider.Setup(x => x.GetOffersAsync())
                .ReturnsAsync(offers);
            fixture.Inject<IEnumerable<IDataProvider>>(new[] { provider.Object });
        }

        [Test]
        public async Task RunAsync_WhenDbIsEmpty_ThenAllOffersAreNew()
        {
            var dal = fixture.FreezeMock<IOffersDal>();
            dal.Setup(x => x.GetAllAsync())
                .ReturnsAsync(new Offer[0]);

            MockPoll(TestData.PollSomeOffers().ToArray());

            var notifications = fixture.FreezeMock<INotificationsManager>();

            var sut = CreateSut();

            await sut.RunAsync();

            dal.Verify(x => x.AddAsync(It.IsNotNull<Offer>()), Times.Exactly(3));
        }
    }
}
