using AngleSharp;
using webapi.Data;
using static webapi.Common.GlobalConstants;

namespace webapi.Services.Seeding
{
    public class SeederService : ISeederService
    {
        private readonly RainbowSixSiegeDbContext _context;
        private readonly IBrowsingContext _browsingContext;

        public SeederService(RainbowSixSiegeDbContext context)
        {
            _context = context;
            _browsingContext = new BrowsingContext(ScraperConfig);
        }

        public Task SeedAsync()
        {
            throw new NotImplementedException();
        }

        /// <returns>A dictionary with the operator link as key and photo as value</returns>
        private async Task<Dictionary<string, string>> GetOperatorLinks()
        {
            using var document = await _browsingContext.OpenAsync(OperatorsBaseUrl);
            var links = document.QuerySelectorAll(".oplist__cards__wrapper div a").Select(x => OperatorsBaseUrl + x.GetAttribute("href")?.Split('/').Last());
            var photos = document.QuerySelectorAll(".oplist__card__img").Select(x => x.GetAttribute("src"));
            return links.Zip(photos).ToDictionary()!;
        }
    }
}
