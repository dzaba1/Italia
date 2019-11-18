using System;
using System.Collections.Generic;

namespace Italia.Lib.Model
{
    public struct ReferenceKey : IEquatable<ReferenceKey>
    {
        public ReferenceKey(string dataProvider, string externalReference)
        {
            DataProvider = dataProvider;
            ExternalReference = externalReference;
        }

        public string DataProvider { get; }
        public string ExternalReference { get; }

        public override bool Equals(object obj)
        {
            return obj is ReferenceKey key && Equals(key);
        }

        public bool Equals(ReferenceKey other)
        {
            return string.Equals(DataProvider, other.DataProvider, StringComparison.OrdinalIgnoreCase) &&
                   ExternalReference == other.ExternalReference;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 398999163;
                hashCode = hashCode * -1521134295 + StringComparer.OrdinalIgnoreCase.GetHashCode(DataProvider);
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ExternalReference);
                return hashCode;
            }
            
        }

        public static bool operator ==(ReferenceKey left, ReferenceKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ReferenceKey left, ReferenceKey right)
        {
            return !(left == right);
        }
    }
}
