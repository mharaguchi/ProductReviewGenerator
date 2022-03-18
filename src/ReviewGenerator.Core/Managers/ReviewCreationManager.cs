using ReviewGenerator.Core.Models;
using ReviewGenerator.Core.Services;

namespace ReviewGenerator.Core.Managers
{
    public class ReviewCreationManager : IReviewCreationManager
    {
        private readonly IReviewGenerator _reviewGenerator;
        public ReviewCreationManager(IReviewGenerator reviewGenerator)
        {
            _reviewGenerator = reviewGenerator;
        }

        public GeneratedReview Generate()
        {
            var review = _reviewGenerator.GenerateReview();
            return review;
        }
    }
}
