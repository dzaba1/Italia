using System.Linq;
using System.Text;
using Italia.Lib.Model;

namespace Italia.Lib.Notifications.Email
{
    public interface IEmailBodyBuilder
    {
        EmailBody Generate(OffersToNotify offers);
    }

    internal sealed class TextEmailBodyBuilder : IEmailBodyBuilder
    {
        public EmailBody Generate(OffersToNotify offers)
        {
            var builder = new StringBuilder();

            if (offers.NewOffers.Any())
            {
                builder.AppendLine("Nowe oferty:");
                builder.AppendLine();

                foreach (var offer in offers.NewOffers)
                {
                    builder.AppendLine(FormatOffer(offer));
                    builder.AppendLine();
                }
            }

            if (offers.ActiveAgainOffers.Any())
            {
                builder.AppendLine("Znowu aktywne oferty:");
                builder.AppendLine();

                foreach (var offer in offers.ActiveAgainOffers)
                {
                    builder.AppendLine(FormatOfferPair(offer));
                    builder.AppendLine();
                }
            }

            if (offers.ChangedOffers.Any())
            {
                builder.AppendLine("Zmienione oferty:");
                builder.AppendLine();

                foreach (var offer in offers.ChangedOffers)
                {
                    builder.AppendLine(FormatOfferPair(offer));
                    builder.AppendLine();
                }
            }

            if (offers.GoneOffers.Any())
            {
                builder.AppendLine("Skończone oferty:");
                builder.AppendLine();

                foreach (var offer in offers.GoneOffers)
                {
                    builder.AppendLine(FormatOffer(offer));
                    builder.AppendLine();
                }
            }

            return new EmailBody
            {
                IsHtml = false,
                Body = builder.ToString().TrimEnd()
            };
        }

        private string FormatOfferPair(OffersPair offer)
        {
            var builder = new StringBuilder();

            builder.AppendLine("Było:");
            builder.AppendLine(FormatOffer(offer.OldOffer));
            builder.AppendLine();

            builder.AppendLine("Jest:");
            builder.AppendLine(FormatOffer(offer.NewOffer));

            return builder.ToString().TrimEnd();
        }

        private string FormatOffer(Offer offer)
        {
            var builder = new StringBuilder();
        
            builder.AppendLine($"Państwo: {offer.Country}");
            builder.AppendLine($"Hotel: {offer.HotelName}");
            builder.AppendLine($"Cena: {offer.Price}");
            if (offer.OriginalPrice.HasValue)
            {
                builder.AppendLine($"Oryginalna cena: {offer.OriginalPrice.Value}");
            }
            builder.AppendLine($"Oferta z: {offer.Created}");
            builder.AppendLine($"Ostatni raz zmieniona: {offer.Modified}");
            builder.AppendLine($"Od: {offer.From}");
            builder.AppendLine($"Do: {offer.To}");
            builder.AppendLine($"Ocena: {offer.Rating}");
            builder.AppendLine($"Odlot z: {offer.Departure}");
            builder.AppendLine($"Link: {offer.Url}");

            return builder.ToString().TrimEnd();
        }
    }
}
