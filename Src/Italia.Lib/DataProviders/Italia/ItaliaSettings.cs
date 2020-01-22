using System;

namespace Italia.Lib.DataProviders.Italia
{
    public interface IItaliaSettings
    {
        Uri Url { get; }
        string ReferencePropertyName { get; }
    }

    internal sealed class ItaliaSettings : IItaliaSettings
    {
        public Uri Url { get; set; }

        public string ReferencePropertyName { get; set; }
    }
}
