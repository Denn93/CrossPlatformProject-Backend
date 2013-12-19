using CrawlerBatch.MatchyBackEnd;

namespace CrawlerBatch.Mappers
{
    public abstract class AMapper<TDao, TServicedao>
    {
        protected MatchyService MatchyBackend;

        protected AMapper()
        {
            MatchyBackend = new MatchyService();
        }

        public abstract int Insert(TDao obj);

        public abstract TDao[] Get(int id = 0);

        public abstract TServicedao MapToService(TDao education);

        public abstract TDao MapFromService(TServicedao education);

    }
}
