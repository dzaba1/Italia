using AutoFixture;
using Dzaba.TestUtils;
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


    }
}
