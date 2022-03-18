using ReviewGenerator.Core.Models;

namespace ReviewGenerator.Core.Services
{
    public interface IWeightedChoiceProvider
    {
        public string GetNextWordByWeight(Dictionary<string, int> choices);

        public string GetNextWordByWeight(List<MarkovWordCount> choices);
    }
}
