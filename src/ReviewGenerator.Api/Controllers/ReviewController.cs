using ReviewGenerator.Core.Managers;
using ReviewGenerator.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace ReviewGenerator.Api.Controllers
{
    [ApiController]
    [Route("API")]
    public class ReviewController : ControllerBase
    {
        private readonly ILogger<ReviewController> _logger;
        private readonly IReviewCreationManager _manager;

        public ReviewController(ILogger<ReviewController> logger, IReviewCreationManager manager)
        {
            _logger = logger;
            _manager = manager;
        }

        [HttpGet("generate")]
        public GeneratedReview Get()
        {
            return _manager.Generate();
        }
    }
}