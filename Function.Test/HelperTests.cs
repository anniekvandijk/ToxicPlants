using Function.Utilities;
using NUnit.Framework;

namespace Function.Tests
{
    [TestFixture]
    internal class HelperTests
    {
        [Test]
        [TestCase(null, null)]
        [TestCase("", null)]
        [TestCase("  ", null)]
        [TestCase(" bla   ", "bla")]
        public void Helper_TrimToNull_ReturnsNullOrString(string stringValue, string expectedResult)
        {
            // Act
            var actualResult = stringValue.TrimToNull();
            
            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
