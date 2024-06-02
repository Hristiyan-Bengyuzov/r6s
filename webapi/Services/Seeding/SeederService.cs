using AngleSharp;
using AngleSharp.Dom;
using webapi.Data;
using webapi.Data.Models;
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

        public async Task SeedAsync()
        {
            if (_context.Operators.Any()) return;

            foreach (var (link, photo) in await GetOperatorLinks())
            {
                using var document = await _browsingContext.OpenAsync(link);
                string icon = document.QuerySelector(".operator__header__icons__names img")!.GetAttribute("src")!;
                string name = document.QuerySelector(".operator__header__icons__names h1")!.TextContent;
                string side = document.QuerySelector(".operator__header__side__detail span")!.TextContent;

                // list containing hp, speed and difficulty
                var stats = document.QuerySelectorAll(".react-rater").Select(r => r.QuerySelectorAll(".is-active").Count()).ToList();

                // Zero doesn't have his difficulty stat. Fuck you Ubisoft
                if (stats.Count == 2) stats.Add(1);

                (int hp, int speed, int difficulty) = (stats[0], stats[1], stats[2]);

                var loadOutItems = document.QuerySelectorAll(".operator__loadout__category__items");

                string? ability = null;
                string? abilityPhoto = null;
                if (loadOutItems.Count() == 4)
                {
                    ability = loadOutItems[3].QuerySelector("p")?.TextContent;
                    abilityPhoto = loadOutItems[3].QuerySelector("img")?.GetAttribute("src");
                }

                var op = new Operator
                {
                    Name = name,
                    Photo = photo,
                    Icon = icon,
                    Side = side,
                    Health = hp,
                    Speed = speed,
                    Difficulty = difficulty,
                    Ability = ability,
                    AbilityPhoto = abilityPhoto
                };

                await _context.Operators.AddAsync(op);
                await _context.SaveChangesAsync();

                var weapons = new List<Weapon>();
                var gadgets = new List<Gadget>();

                var scrapeLoadOut = (IHtmlCollection<IElement> htmlCollection, LoadOutType loadOutType, string? weaponType = null) =>
                {
                    foreach (var item in htmlCollection)
                    {
                        var loadOutInfo = item.QuerySelectorAll("p");
                        (string loadOutName, string loadOutCategory) = (loadOutInfo[0].TextContent, loadOutInfo[1].TextContent);
                        string loadOutPhoto = item.QuerySelector("img")!.GetAttribute("src")!;

                        if (loadOutType == LoadOutType.Weapon)
                        {
                            var existingWeapon = _context.Weapons.FirstOrDefault(w => w.Name == loadOutName);
                            if (existingWeapon is null)
                            {
                                var weapon = new Weapon
                                {
                                    Name = loadOutName,
                                    Category = loadOutCategory,
                                    Photo = loadOutPhoto,
                                    Type = weaponType!,
                                };
                                _context.Weapons.Add(weapon);
                                weapons.Add(weapon);
                            }
                            else
                            {
                                weapons.Add(existingWeapon);
                            }
                        }
                        else // gadget
                        {
                            var existingGadget = _context.Gadgets.FirstOrDefault(g => g.Name == loadOutName);
                            if (existingGadget is null)
                            {
                                var gadget = new Gadget
                                {
                                    Name = loadOutName,
                                    Photo = loadOutPhoto,
                                };
                                _context.Gadgets.Add(gadget);
                                gadgets.Add(gadget);
                            }
                            else
                            {
                                gadgets.Add(existingGadget);
                            }
                        }
                    }
                };

                scrapeLoadOut(loadOutItems[0].QuerySelectorAll(".operator__loadout__weapon"), LoadOutType.Weapon, "Primary");
                scrapeLoadOut(loadOutItems[1].QuerySelectorAll(".operator__loadout__weapon"), LoadOutType.Weapon, "Secondary");
                scrapeLoadOut(loadOutItems[2].QuerySelectorAll(".operator__loadout__weapon"), LoadOutType.Gadget);

                await _context.SaveChangesAsync();
            }
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