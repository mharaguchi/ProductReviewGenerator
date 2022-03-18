using ReviewGenerator.Core.Repositories;
using Markov;
using Microsoft.Extensions.Logging;

namespace ReviewGenerator.Core.Services
{
    public class MarkovChainNuGetReviewTextGenerator : IReviewTextGenerator
    {
        MarkovChain<string> _chain = new MarkovChain<string>(1);

        private readonly IReviewRepository _reviewRepository;
        private readonly ILogger<MarkovChainNuGetReviewTextGenerator> _logger;

        public MarkovChainNuGetReviewTextGenerator(ILogger<MarkovChainNuGetReviewTextGenerator> logger, IReviewRepository reviewRepository)
        {
            _logger = logger;
            _reviewRepository = reviewRepository;
        
            _logger.LogInformation("Starting GetReviews");
            var reviews = _reviewRepository.GetReviews();
            _logger.LogInformation("Finished GetReviews");

            _logger.LogInformation("Starting data training");
            this.SeedWordFrequencyList(reviews);
            _logger.LogInformation("Finished data training");
        }

        #region IReviewTextGenerator
        public string GenerateReviewText()
        {
            var rand = new Random();

            var reviewText = string.Join(" ", _chain.Chain(rand));
            return reviewText;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Sets up the dictionary that maps words to the words that come after, by count
        /// </summary>
        /// <param name="reviews"></param>
        private void SeedWordFrequencyList(List<string> reviews)
        {
            foreach (var review in reviews)
            {
                var words = GetReviewWords(review);
                if (words.Length == 0)
                {
                    continue;
                }

                _chain.Add(words, 1);
            }
        }

        private string[] GetReviewWords(string review)
        {
            var words = review.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            return words;
        }
        #endregion
    }
}
