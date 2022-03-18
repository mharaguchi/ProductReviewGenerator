namespace ReviewGenerator.Core.Services
{
    public interface IStarsGenerator
    {
        public int GetStarsNumber(string reviewText);
    }
}
