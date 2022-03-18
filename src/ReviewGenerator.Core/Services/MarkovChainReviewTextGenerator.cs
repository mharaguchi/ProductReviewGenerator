using ReviewGenerator.Core.Extensions;
using ReviewGenerator.Core.Models;
using ReviewGenerator.Core.Repositories;
using Microsoft.Extensions.Logging;
using System.Text;

namespace ReviewGenerator.Core.Services
{
    public class MarkovChainReviewTextGenerator : IReviewTextGenerator
    {
        const int ALLOWABLE_SENTENCE_DIFFERENTIAL = 2;

        private readonly Dictionary<string, int> startingWordCounts = new();

        /// <summary>
        /// Maps a word to a list of words that have appeared after that word and the number of times they appeared
        /// </summary>
        private readonly Dictionary<string, List<MarkovWordCount>> wordCountMap = new();

        /// <summary>
        /// Maps a word to a list of words that have appeared after that word and the percentage chance of that word appearing
        /// </summary>
        //private readonly Dictionary<string, List<MarkovWordOption>> wordOptionMap = new(); 

        private readonly IReviewRepository _reviewRepository;
        private readonly IWeightedChoiceProvider _weightedChoiceProvider;
        private readonly IReviewDataInfoProvider _reviewDataInfoProvider;
        private readonly ILogger<MarkovChainReviewTextGenerator> _logger;

        public MarkovChainReviewTextGenerator(ILogger<MarkovChainReviewTextGenerator> logger, IReviewRepository reviewRepository, IWeightedChoiceProvider weightedChoiceProvider, IReviewDataInfoProvider reviewDataInfoProvider)
        {
            _logger = logger;
            _reviewRepository = reviewRepository;
            _weightedChoiceProvider = weightedChoiceProvider;
            _reviewDataInfoProvider = reviewDataInfoProvider;

            _logger.LogInformation("Starting GetReviews");
            var reviews = _reviewRepository.GetReviews();
            _logger.LogInformation("Finished GetReviews");

            _logger.LogInformation("Starting data training");
            this.SeedWordFrequencyList(reviews);
            _logger.LogInformation("Finished data training");
        }

        private string GetNextWord(string prevWord)
        {
            string? nextWord;
            if (wordCountMap.ContainsKey(prevWord))
            {
                nextWord = _weightedChoiceProvider.GetNextWordByWeight(wordCountMap[prevWord]);
            }
            else //if previous word has no options
            {
                nextWord = ".";
            }

            return nextWord;
        }

        #region IReviewTextGenerator
        public string GenerateReviewText()
        {
            var averageSentences = _reviewDataInfoProvider.GetAverageReviewLengthBySentences();
            var numSentences = GetNumSentences(averageSentences);
            var reviewTextBuilder = new StringBuilder();

            for(int i = 0; i < numSentences; i++)
            {
                var sentence = GenerateSentence();
                reviewTextBuilder.Append(sentence);
                if (i < numSentences - 1)
                {
                    reviewTextBuilder.Append(' ');
                }
            }

            return reviewTextBuilder.ToString();
        }
        #endregion

        #region Private Methods

        private int GetNumSentences(int averageSentences)
        {
            var diff = ALLOWABLE_SENTENCE_DIFFERENTIAL;
            var random = new Random();

            if (averageSentences < 3)
            {
                return random.Next(1, 4);
            }

            var min = averageSentences - diff;
            var increment = random.Next(-1 * diff, diff + 1);

            return min + increment;
        }

        private string GenerateSentence()
        {
            var sentenceBuilder = new StringBuilder();
            var startingWord = _weightedChoiceProvider.GetNextWordByWeight(startingWordCounts);
            sentenceBuilder.Append(startingWord);
            
            var word = "";
            var prevWord = startingWord;
            while (!word.IsEndOfSentence())
            {
                word = GetNextWord(prevWord);
                if (!word.IsEndOfSentence())
                {
                    sentenceBuilder.Append(' ');
                }
                sentenceBuilder.Append(word);
                prevWord = word;
            }

            return sentenceBuilder.ToString();
        }

        /// <summary>
        /// Sets up the dictionary that maps words to the words that come after, by count
        /// </summary>
        /// <param name="reviews"></param>
        private void SeedWordFrequencyList(List<string> reviews)
        {
            var totalSentences = 0;

            foreach (var review in reviews)
            {
                var words = GetReviewWords(review);
                if (words.Length == 0)
                {
                    continue;
                }

                totalSentences++;
                CountStartingWord(words[0]);

                for (int i = 1; i < words.Length; i++)
                {
                    var prevWord = words[i - 1];
                    var currentWord = words[i];
                    if (wordCountMap.ContainsKey(prevWord))
                    {
                        var wordFrequencyList = wordCountMap[prevWord];
                        CountWordOccurrence(wordFrequencyList, currentWord);
                    }
                    else
                    {
                        wordCountMap.Add(prevWord, new List<MarkovWordCount> { new MarkovWordCount { Word = currentWord, Occurrences = 1 } });
                    }

                    if (prevWord.IsEndOfSentence())
                    {
                        totalSentences++;
                        if (!currentWord.IsEndOfSentence()) //Filter out the ... or !!! types of entries
                        {
                            CountStartingWord(currentWord);
                        }
                    }
                }
            }
            SetAverageReviewLength(totalSentences, reviews.Count);
        }

        private void SetAverageReviewLength(int totalSentences, int numReviews)
        {
            int averageSentences = (int)Math.Round((decimal)totalSentences / numReviews);
            var success = _reviewDataInfoProvider.SetAverageReviewLengthInSentences(averageSentences);
            if (!success)
            {
                //TODO: log error
            }
        }

        private string[] GetReviewWords(string review)
        {
            var cleanedReview = review;
            cleanedReview = cleanedReview.Replace(".", " . ");
            cleanedReview = cleanedReview.Replace("!", " ! ");
            var separators = new char[] { '\r', '\n', '-', '(', ')', ',','&',':',' ' };

            var words = cleanedReview.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            return words;
        }

        private void CountWordOccurrence(List<MarkovWordCount> wordFrequencyList, string currentWord)
        {
            var thisWordFrequency = wordFrequencyList.Where(x => x.Word == currentWord).FirstOrDefault();
            if (thisWordFrequency != null)
            {
                thisWordFrequency.Occurrences++;
            }
            else
            {
                wordFrequencyList.Add(new MarkovWordCount { Word = currentWord, Occurrences = 1 });
            }
        }

        private void CountStartingWord(string word)
        {
            if (startingWordCounts.ContainsKey(word))
            {
                startingWordCounts[word]++;
            }
            else
            {
                startingWordCounts.Add(word, 1);
            }
        }
        #endregion
    }
}
