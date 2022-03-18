using ReviewGenerator.Core.Models;

namespace ReviewGenerator.Core.Services
{
    public class StarsAndTextReviewGenerator : IReviewGenerator
    {
        private readonly IReviewTextGenerator _reviewTextGenerator;
        private readonly IStarsGenerator _starsGenerator;

        public StarsAndTextReviewGenerator(IReviewTextGenerator reviewTextGenerator, IStarsGenerator starsGenerator)
        {
            _starsGenerator = starsGenerator;
            _reviewTextGenerator = reviewTextGenerator;
        }

        public GeneratedReview GenerateReview()
        {
            var reviewText = _reviewTextGenerator.GenerateReviewText();
            var stars = _starsGenerator.GetStarsNumber(reviewText);

            return new GeneratedReview { ReviewText = reviewText, StarRating = stars};
        }
    }
}
