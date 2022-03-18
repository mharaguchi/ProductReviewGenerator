namespace ReviewGenerator.Core.Services
{
    public interface IReviewDataInfoProvider
    {
        public int GetAverageReviewLengthBySentences();
        public bool SetAverageReviewLengthInSentences(int averageReviewLength);
    }
}
