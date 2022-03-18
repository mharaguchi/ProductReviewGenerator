using ReviewGenerator.Core.Models;
using ReviewGenerator.Core.Repositories;
using ReviewGenerator.Data.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ReviewGenerator.Data
{
    public class FileSystemReviewRepository : IReviewRepository
    {
        private readonly IOptions<FilesystemReviewRepositoryOptions> _options;

        public FileSystemReviewRepository(IOptions<FilesystemReviewRepositoryOptions> options)
        {
            _options = options;
        }

        public List<string> GetReviews()
        {
            var reviews = new List<string>();

            foreach (string line in System.IO.File.ReadLines(_options.Value.FilePath))
            {
                if (line.Trim().Length > 0)
                {
                    try
                    {
                        var amazonReview = JsonSerializer.Deserialize<Amazon5CoreReviewJson>(line);
                        if (amazonReview != null)
                        {
                            reviews.Add(amazonReview?.reviewText);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            return reviews;
        }
    }
}