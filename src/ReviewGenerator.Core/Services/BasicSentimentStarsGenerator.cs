using ReviewGenerator.Core.Models;
using Microsoft.Extensions.Options;

namespace ReviewGenerator.Core.Services
{
    public class BasicSentimentStarsGenerator : IStarsGenerator
    {
        private readonly string[] POSITIVE_WORDS = { "good", "great", "love", "loved", "like", "liked", "best", "durable", "enjoy", "fun", "beautiful", "excellent" };
        private readonly string[] NEGATIVE_WORDS = { "bad", "poor", "broke", "dislike", "disliked", "hate", "hated", "worst", "boring", "lame", "ugly", "awful", "expensive", "junk", "trash", "garbage", "worthless" };

        private readonly IOptions<StarsGeneratorOptions> _options;

        public BasicSentimentStarsGenerator(IOptions<StarsGeneratorOptions> options)
        {
            _options = options;
        }

        public int GetStarsNumber(string reviewText)
        {
            var words = reviewText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var ratio = GetRatio(words);

            var stars = CalculateStarsNumber(ratio, _options.Value.Min, _options.Value.Max);
            
            return stars;
        }

        private int CalculateStarsNumber(decimal ratio, int min, int max)
        {
            var adjustedMax = max - min;
            var adjustedScore = adjustedMax * ratio;
            var roundedScore = (int)Math.Round(adjustedScore);
            var finalScore = roundedScore + min;

            return finalScore;
        }

        private decimal GetRatio(string[] words)
        {
            decimal ratio;

            var positiveCount = 0;
            var negativeCount = 0;

            foreach (var word in words)
            {
                var lowerWord = word.ToLower();
                if (POSITIVE_WORDS.Contains(lowerWord))
                {
                    positiveCount++;
                }
                else if (NEGATIVE_WORDS.Contains(lowerWord))
                {
                    negativeCount++;
                }
            }
            
            var total = positiveCount + negativeCount;
            ratio = total == 0 ? 0.5m : (decimal)positiveCount / total;

            return ratio;
        }
    }
}
