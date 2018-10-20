namespace Tests.Model.Climate
{
    using NUnit.Framework;

    [TestFixture]
    public class ClimateTests
    {
        [OneTimeSetUp]
        public void SetUp()
        {
        }

        [Test]
        public void ValidateValidTest()
        {
        }

        [TestCase(false, true)]
        [TestCase(true, false)]
        [TestCase(null, true)]
        public void ValidateInvalidTest(bool? useValidEffect, bool useParser)
        {
        }
    }
}