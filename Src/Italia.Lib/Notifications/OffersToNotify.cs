using System.Collections.Generic;
using Dzaba.Utils;
using Italia.Lib.Model;

namespace Italia.Lib.Notifications
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
        private List<Offer> newOffers = new List<Offer>();
        private List<Offer> activeAgainOffers = new List<Offer>();
        private List<Offer> goneOffers = new List<Offer>();
        private List<OffersPair> changedOffers = new List<OffersPair>();

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

        public void AddOfferActiveAgain(Offer offer)
        {
            Require.NotNull(offer, nameof(offer));

            activeAgainOffers.Add(offer);
        }

        public void AddOfferIsGone(Offer offer)
        {
            Require.NotNull(offer, nameof(offer));

            goneOffers.Add(offer);
        }
    }
}
