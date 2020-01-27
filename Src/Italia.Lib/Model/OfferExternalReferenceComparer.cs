using System;
using System.Collections.Generic;

namespace Italia.Lib.Model
{
    internal sealed class OfferExternalReferenceComparer : IEqualityComparer<Offer>
    {
        public static readonly OfferExternalReferenceComparer Instance = new OfferExternalReferenceComparer();

        public bool Equals(Offer x, Offer y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return string.Equals(x.ExternalReference, y.ExternalReference, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(Offer obj)
        {
            return (obj.ExternalReference != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(obj.ExternalReference) : 0);
        }
    }
}