using ReviewGenerator.Core.Models;
using Microsoft.Extensions.Options;

namespace ReviewGenerator.Core.Services
{
    public class RandomStarsGenerator : IStarsGenerator
    {
        private readonly Random _random;
        private readonly IOptions<StarsGeneratorOptions> _options;

        public RandomStarsGenerator(IOptions<StarsGeneratorOptions> options)
        {
            _options = options;
            _random = new Random();
        }

        public int GetStarsNumber(string reviewText)
        {
            return _random.Next(_options.Value.Min, _options.Value.Max + 1);
        }
    }
}
