using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.IO;
using System.Reflection;

namespace Function.Tests.Utilities
{
    internal static class Helpers
    {
        public static void VerifyLog<T>(this Mock<ILogger<T>> mockLogger, LogLevel loglevel, string message)
        {
            mockLogger.Verify(logger => logger.Log(
                    It.Is<LogLevel>(logLevel => logLevel == loglevel),
                    It.Is<EventId>(eventId => eventId.Id == 0),
                    It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == message && @type.Name == "FormattedLogValues"),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        public static Stream CreateFileStream(string imageName)
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ??
                                    throw new InvalidOperationException("File not found")
                , "Data", imageName);

            FileStream fileStream = new(path, FileMode.Open, FileAccess.Read);
            return fileStream;
        }
    }
}
