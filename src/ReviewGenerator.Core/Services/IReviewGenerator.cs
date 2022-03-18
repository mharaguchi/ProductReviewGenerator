using ReviewGenerator.Core.Models;

namespace ReviewGenerator.Core.Services
{
    public interface IReviewGenerator
    {
        public GeneratedReview GenerateReview();
    }
}
