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
            fixture.Inject<IEnumerable<IDataProvider>>(new[] {provider.Object});
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

            dal.Verify(x => x.AddAsync(It.Is<Offer>(o => o.Active)), Times.Exactly(3));
            dal.Verify(x => x.UpdateAsync(It.IsAny<Offer>()), Times.Never);

            notifications.Verify(
                x => x.NotifyAsync(It.Is<OffersToNotify>(o =>
                    o.NewOffers.Count() == 3 && !o.ActiveAgainOffers.Any() && !o.ChangedOffers.Any() &&
                    !o.GoneOffers.Any())), Times.Once());
        }

        [Test]
        public async Task RunAsync_WhenRecordsExist_ThenNothingHappens()
        {
            var offers = TestData.PollSomeOffers().ToArray();

            var dal = fixture.FreezeMock<IOffersDal>();
            dal.Setup(x => x.GetAllAsync())
                .ReturnsAsync(offers);

            MockPoll(offers);

            var notifications = fixture.FreezeMock<INotificationsManager>();

            var sut = CreateSut();

            await sut.RunAsync();

            dal.Verify(x => x.AddAsync(It.IsAny<Offer>()), Times.Never);
            dal.Verify(x => x.UpdateAsync(It.IsAny<Offer>()), Times.Never);

            notifications.Verify(
                x => x.NotifyAsync(It.Is<OffersToNotify>(o => o.Empty)), Times.Once());
        }

        [Test]
        public async Task RunAsync_WhenExistingRecordsAreNotPolled_ThenTheyAreNotActive()
        {
            var offers = TestData.PollSomeOffers().ToArray();

            var dal = fixture.FreezeMock<IOffersDal>();
            dal.Setup(x => x.GetAllAsync())
                .ReturnsAsync(offers);

            MockPoll();

            var notifications = fixture.FreezeMock<INotificationsManager>();

            var sut = CreateSut();

            await sut.RunAsync();

            dal.Verify(x => x.AddAsync(It.IsAny<Offer>()), Times.Never);
            dal.Verify(x => x.UpdateAsync(It.Is<Offer>(o => !o.Active)), Times.Exactly(3));

            notifications.Verify(
                x => x.NotifyAsync(It.Is<OffersToNotify>(o =>
                    !o.NewOffers.Any() && !o.ActiveAgainOffers.Any() && !o.ChangedOffers.Any() &&
                    o.GoneOffers.Count() == 3)), Times.Once());
        }

        [Test]
        public async Task RunAsync_WhenRecordsChanged_ThenCorrectNotificationIsShown()
        {
            var dal = fixture.FreezeMock<IOffersDal>();
            dal.Setup(x => x.GetAllAsync())
                .ReturnsAsync(TestData.PollSomeOffers().ToArray());

            MockPoll(TestData.PollSomeOffers()
                .Select(o =>
                {
                    o.Price += 100;
                    return o;
                })
                .ToArray());

            var notifications = fixture.FreezeMock<INotificationsManager>();

            var sut = CreateSut();

            await sut.RunAsync();

            dal.Verify(x => x.AddAsync(It.IsAny<Offer>()), Times.Never);
            dal.Verify(x => x.UpdateAsync(It.Is<Offer>(o => o.Active)), Times.Exactly(3));

            notifications.Verify(
                x => x.NotifyAsync(It.Is<OffersToNotify>(o =>
                    !o.NewOffers.Any() && !o.ActiveAgainOffers.Any() && o.ChangedOffers.Count() == 3 &&
                    !o.GoneOffers.Any())), Times.Once());
        }

        [Test]
        public async Task RunAsync_WhenRecordsAppearAgain_ThenTheyAreActive()
        {
            var dal = fixture.FreezeMock<IOffersDal>();
            dal.Setup(x => x.GetAllAsync())
                .ReturnsAsync(TestData.PollSomeOffers()
                .Select(o =>
                {
                    o.Active = false;
                    return o;
                })
                .ToArray());

            MockPoll(TestData.PollSomeOffers()
                .ToArray());

            var notifications = fixture.FreezeMock<INotificationsManager>();

            var sut = CreateSut();

            await sut.RunAsync();

            dal.Verify(x => x.AddAsync(It.IsAny<Offer>()), Times.Never);
            dal.Verify(x => x.UpdateAsync(It.Is<Offer>(o => o.Active)), Times.Exactly(3));

            notifications.Verify(
                x => x.NotifyAsync(It.Is<OffersToNotify>(o =>
                    !o.NewOffers.Any() && o.ActiveAgainOffers.Count() == 3 && !o.ChangedOffers.Any() &&
                    !o.GoneOffers.Any())), Times.Once());
        }
    }
}
