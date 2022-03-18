using ReviewGenerator.Core.Models;
using ReviewGenerator.Core.Services;
using Microsoft.Extensions.Options;
using Xunit;

namespace ReviewGenerator.Core.Tests.Services
{
    public class BasicSentimentStarsGeneratorTests
    {
        const int STARS_MIN = 1;
        const int STARS_MAX = 5;

        private readonly BasicSentimentStarsGenerator _starsGenerator;
        private readonly IOptions<StarsGeneratorOptions> _options;

        public BasicSentimentStarsGeneratorTests()
        {
            var starsGeneratorOptions = new StarsGeneratorOptions
            {
                Min = STARS_MIN,
                Max = STARS_MAX
            };
            _options = Options.Create<StarsGeneratorOptions>(starsGeneratorOptions);
            _starsGenerator = new BasicSentimentStarsGenerator(_options);
        }

        [Theory]
        [InlineData("", 1, 5, 3)]
        [InlineData("", 2, 6, 4)]
        [InlineData("blahblahblah", 1, 5, 3)]
        [InlineData("blah blah blah.", 1, 5, 3)]
        [InlineData("good love.", 1, 5, 5)]
        [InlineData("good love.", 3, 9, 9)]
        [InlineData("good love.", 2, 10, 10)]
        [InlineData("bad hated", 1, 5, 1)]
        [InlineData("bad hated", 3, 9, 3)]
        [InlineData("bad hated", 2, 10, 2)]
        [InlineData("good great awful bad hated", 1, 5, 3)]
        [InlineData("good great best loved hated", 1, 5, 4)]
        public void GetStarsNumber_SentimentDriven_ReturnsCorrectValue(string reviewText, int min, int max, int expected)
        {
            //arrange
            var starsGeneratorOptions = new StarsGeneratorOptions
            {
                Min = min,
                Max = max
            };
            var localOptions = Options.Create<StarsGeneratorOptions>(starsGeneratorOptions);
            var starsGenerator = new BasicSentimentStarsGenerator(localOptions);

            //act
            var val = starsGenerator.GetStarsNumber(reviewText);

            //assert
            Assert.Equal(expected, val);
        }
    }
}
