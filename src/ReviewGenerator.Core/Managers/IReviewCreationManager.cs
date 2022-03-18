using ReviewGenerator.Core.Models;

namespace ReviewGenerator.Core.Managers
{
    public interface IReviewCreationManager
    {
        public GeneratedReview Generate();
    }
}
