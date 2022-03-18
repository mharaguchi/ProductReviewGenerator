using ReviewGenerator.Core.Models;

namespace ReviewGenerator.Core.Services
{
    public class WeightedChoiceProvider : IWeightedChoiceProvider
    {
        public string GetNextWordByWeight(Dictionary<string, int> choices)
        {
            var totalWeight = choices.Sum(x => x.Value);
            var random = new Random();
            var selectedRandom = random.Next(0, totalWeight + 1);
            var currentMatchTarget = selectedRandom;

            foreach(var kvp in choices)
            {
                var thisWordWeight = kvp.Value;
                if (currentMatchTarget < thisWordWeight)
                {
                    return kvp.Key;
                }
                currentMatchTarget -= thisWordWeight;
            }

            //TODO: log error
            return "";
        }

        public string GetNextWordByWeight(List<MarkovWordCount> choices)
        {
            var totalWeight = choices.Sum(x => x.Occurrences);
            var random = new Random();
            var selectedRandom = random.Next(0, totalWeight);
            var currentMatchTarget = selectedRandom;

            foreach (var wordCount in choices)
            {
                var thisWordWeight = wordCount.Occurrences;
                if (currentMatchTarget < thisWordWeight)
                {
                    return wordCount.Word;
                }
                currentMatchTarget -= thisWordWeight;
            }

            //TODO: Log error
            return "";
        }
    }
}
