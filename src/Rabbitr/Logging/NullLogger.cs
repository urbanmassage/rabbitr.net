using rabbitr.net.Abstractions;

namespace Rabbitr.Logging
{
    public class NullLogger : IRabbitrLogging
    {
        public void LogCritical(string message)
        {
        }

        public void LogError(string message)
        {
        }

        public void LogInformation(string message)
        {
        }

        public void LogWarning(string message)
        {
        }
    }
}