using AngleSharp;
using IConfiguration = AngleSharp.IConfiguration;

namespace webapi.Common
{
    public static class GlobalConstants
    {
        public const string OperatorsBaseUrl = "https://www.ubisoft.com/en-gb/game/rainbow-six/siege/game-info/operators/";
        public static IConfiguration ScraperConfig = Configuration.Default.WithDefaultLoader();
    }
}
