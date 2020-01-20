using System.Collections.Generic;
using Dzaba.Utils;
using Italia.Lib.Model;

namespace Italia.Lib
{
    public sealed class OffersPair
    {
        public OffersPair(Offer newOffer, Offer oldOffer)
        {
            Require.NotNull(oldOffer, nameof(oldOffer));
            Require.NotNull(newOffer, nameof(newOffer));

            NewOffer = newOffer;
            OldOffer = oldOffer;
        }

        public Offer NewOffer { get; }
        public Offer OldOffer { get; }
    }

    public sealed class OffersToNotify
    {
        private readonly List<Offer> newOffers = new List<Offer>();
        private readonly List<OffersPair> activeAgainOffers = new List<OffersPair>();
        private readonly List<Offer> goneOffers = new List<Offer>();
        private readonly List<OffersPair> changedOffers = new List<OffersPair>();

        public IEnumerable<Offer> NewOffers => newOffers;
        public IEnumerable<OffersPair> ActiveAgainOffers => activeAgainOffers;
        public IEnumerable<Offer> GoneOffers => goneOffers;
        public IEnumerable<OffersPair> ChangedOffers => changedOffers;

        public void AddNewOffer(Offer offer)
        {
            Require.NotNull(offer, nameof(offer));

            newOffers.Add(offer);
        }

        public void AddOfferChanged(Offer oldOffer, Offer newOffer)
        {
            Require.NotNull(oldOffer, nameof(oldOffer));
            Require.NotNull(newOffer, nameof(newOffer));

            var pair = new OffersPair(newOffer, oldOffer);
            changedOffers.Add(pair);
        }

        public void AddOfferActiveAgain(Offer oldOffer, Offer newOffer)
        {
            Require.NotNull(oldOffer, nameof(oldOffer));
            Require.NotNull(newOffer, nameof(newOffer));

            var pair = new OffersPair(newOffer, oldOffer);
            activeAgainOffers.Add(pair);
        }

        public void AddOfferIsGone(Offer offer)
        {
            Require.NotNull(offer, nameof(offer));

            goneOffers.Add(offer);
        }
    }
}
