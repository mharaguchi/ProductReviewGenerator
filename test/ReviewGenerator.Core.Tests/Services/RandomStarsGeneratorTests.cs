using ReviewGenerator.Core.Models;
using ReviewGenerator.Core.Services;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ReviewGenerator.Core.Tests.Services
{
    public class RandomStarsGeneratorTests
    {
        const int STARS_MIN = 1;
        const int STARS_MAX = 5;

        private readonly RandomStarsGenerator _randomStarsGenerator;
        private readonly IOptions<StarsGeneratorOptions> _options;

        public RandomStarsGeneratorTests()
        {
            var starsGeneratorOptions = new StarsGeneratorOptions
            {
                Min = STARS_MIN,
                Max = STARS_MAX
            };
            _options = Options.Create<StarsGeneratorOptions>(starsGeneratorOptions);
            _randomStarsGenerator = new RandomStarsGenerator(_options);
        }

        [Theory]
        [InlineData("")]
        [InlineData("blahblahblah")]
        [InlineData("blah blah blah.")]
        public void GetStarsNumber_TenThousandRuns_AlwaysWithinRange(string reviewText)
        {
            //arrange
            var starsValues = new List<int>();

            //act
            for (int i = 0; i < 10000; i++)
            {
                starsValues.Add(_randomStarsGenerator.GetStarsNumber(reviewText));
            }

            //assert
            Assert.False(starsValues.Where(x => x < STARS_MIN).Any());
            Assert.False(starsValues.Where(x => x > STARS_MAX).Any());
            Assert.True(starsValues.Where(x => x == STARS_MIN).Any());
            Assert.True(starsValues.Where(x => x == STARS_MAX).Any());
        }
    }
}
