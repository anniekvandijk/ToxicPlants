using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace Function.Tests
{
    [TestClass]
    public class FunctionTests
    {
        [TestMethod]
        public async Task TestFunctionReturningSomething()
        {
            //Arrange
            var request = Mock.Of<HttpRequestData>();
            var executionContext = Mock.Of<FunctionContext>();

            //Act
            var response = await Function.Run(request, executionContext);

            //Assert
            Assert.AreEqual(200, response.StatusCode);
            Assert.AreEqual("Welcome to Azure Functions!", response.Body);
        }
    }
}
