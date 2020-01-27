using System;

namespace Italia.Lib.DataProviders.Italia
{
    public interface IItaliaSettings
    {
        Uri[] Urls { get; }
        string ReferencePropertyName { get; }
    }

    internal sealed class ItaliaSettings : IItaliaSettings
    {
        public Uri[] Urls { get; set; }

        public string ReferencePropertyName { get; set; }
    }
}
