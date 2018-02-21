namespace Rabbitr.Net.Abstractions
{
    public interface IRabbitrLogging
    {
         void LogInformation(string message);
         void LogWarning(string message);
         void LogError(string message);
         void LogCritical(string message);
    }
}