using DeviceDetectorNET;
namespace UrlShortener.Utilities
{
    public class UserAgentHelper
    {
        public static string GetDeviceName(string userAgent)
        {
            var dd = new DeviceDetector(userAgent);
            dd.Parse();

            return dd.GetDeviceName();
        }
    }
}
