using System.Linq;
using System.Threading.Tasks;
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
        private Mock<IDataProvider> dataProviderMock;
        private Mock<IOffersDal> offersDal;
        private Mock<INotificationsManager> notifications;

        [SetUp]
        public void Setup()
        {
            dataProviderMock = new Mock<IDataProvider>();
            offersDal = new Mock<IOffersDal>();
            notifications = new Mock<INotificationsManager>();
        }

        private ItaliaEngine CreateSut()
        {
            return new ItaliaEngine(new[] {dataProviderMock.Object}, offersDal.Object, notifications.Object);
        }

        private void MockPoll(params Offer[] offers)
        {
            dataProviderMock.Setup(x => x.GetOffersAsync())
                .ReturnsAsync(offers);
        }

        [Test]
        public async Task RunAsync_WhenDbIsEmpty_ThenAllOffersAreNew()
        {
            offersDal.Setup(x => x.GetAllAsync())
                .ReturnsAsync(new Offer[0]);

            MockPoll(TestData.PollSomeOffers().ToArray());

            var sut = CreateSut();

            await sut.RunAsync();

            offersDal.Verify(x => x.AddAsync(It.IsNotNull<Offer>()), Times.Exactly(3));
        }
    }
}
