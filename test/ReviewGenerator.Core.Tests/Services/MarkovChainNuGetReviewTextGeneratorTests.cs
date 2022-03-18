using ReviewGenerator.Core.Repositories;
using ReviewGenerator.Core.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace ReviewGenerator.Core.Tests.Services
{
    public class MarkovChainNuGetReviewTextGeneratorTests
    {
        private readonly Mock<ILogger<MarkovChainNuGetReviewTextGenerator>> _logger;
        private readonly Mock<IReviewRepository> _reviewRepository;
        private readonly MarkovChainNuGetReviewTextGenerator _reviewTextGenerator;

        public MarkovChainNuGetReviewTextGeneratorTests()
        {
            _logger = new Mock<ILogger<MarkovChainNuGetReviewTextGenerator>>();
            _reviewRepository = new Mock<IReviewRepository>();
            _reviewRepository.Setup(x => x.GetReviews()).Returns(GetDummyReviews());

            _reviewTextGenerator = new MarkovChainNuGetReviewTextGenerator(_logger.Object, _reviewRepository.Object);
        }

        [Fact]
        public void DummyData_GenerateReviewText_ReturnsString()
        {
            //arrange

            //act
            var reviewText = _reviewTextGenerator.GenerateReviewText();

            //assert
            Assert.NotEmpty(reviewText);
            Assert.Contains("once", reviewText.ToLower());
            _reviewRepository.Verify(x => x.GetReviews(), Times.Once);
        }

        #region
        private List<string> GetDummyReviews()
        {
            return new List<string>
            {
                "Once upon a time.",
                "Once there was a pig.",
                "There once was a man from Nantucket."
            };
        }
        #endregion
    }
}
