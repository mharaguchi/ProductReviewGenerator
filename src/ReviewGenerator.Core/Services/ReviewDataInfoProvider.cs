namespace ReviewGenerator.Core.Services
{
    public class ReviewDataInfoProvider : IReviewDataInfoProvider
    {
        private int _averageReviewLengthInSentences = 0;
        private bool _initialized = false;

        public int GetAverageReviewLengthBySentences()
        {
            if (_initialized)
            {
                return _averageReviewLengthInSentences;
            }

            return -1;
        }

        public bool SetAverageReviewLengthInSentences(int averageReviewLength)
        {
            if (!_initialized)
            {
                _averageReviewLengthInSentences = averageReviewLength;
                _initialized=true;
                return true;
            }

            return false;
        }
    }
}
