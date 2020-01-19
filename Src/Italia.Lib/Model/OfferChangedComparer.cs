using System.Collections.Generic;

namespace Italia.Lib.Model
{
    internal sealed class OfferChangedComparer : IEqualityComparer<Offer>
    {
        public static readonly OfferChangedComparer Instance = new OfferChangedComparer();

        public bool Equals(Offer x, Offer y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return x.Active == y.Active ||
                x.Country == y.Country ||
                x.Departure == y.Departure ||
                x.From == y.From ||
                x.HotelName == y.HotelName ||
                x.OriginalPrice == y.OriginalPrice ||
                x.Price == y.Price ||
                x.To == y.To;
        }

        public int GetHashCode(Offer obj)
        {
            if (obj == null)
            {
                return 0;
            }

            unchecked
            {
                var hashCode = 1542799851;
                hashCode = hashCode * -1521134295 + obj.Active.GetHashCode();
                hashCode = hashCode * -1521134295 + obj.Price.GetHashCode();
                hashCode = hashCode * -1521134295 + EqualityComparer<decimal?>.Default.GetHashCode(obj.OriginalPrice);
                hashCode = hashCode * -1521134295 + obj.From.GetHashCode();
                hashCode = hashCode * -1521134295 + obj.To.GetHashCode();
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(obj.HotelName);
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(obj.Country);
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(obj.Departure);
                return hashCode;
            }
        }
    }
}
