using System;
using DataAccessObjects;

namespace CrawlerBatch.Mappers
{
    public class DetailJobMapper : AMapper<DetailJob, MatchyBackEnd.DetailJob>
    {
        public override int Insert(DetailJob obj)
        {
            int insertId = MatchyBackend.AddDetailJob(MapToService(obj));

            if (insertId == 0)
                Console.WriteLine("DetailJob not inserted. Error, Error");

            return insertId;
        }

        public override DetailJob[] Get(int id = 0)
        {
            throw new NotImplementedException();
        }

        public override MatchyBackEnd.DetailJob MapToService(DetailJob detailJob)
        {
            return new MatchyBackEnd.DetailJob() { Data = detailJob.Data, DetailJobId = detailJob.DetailJobId, JobId = detailJob.JobId };
        }

        public override DetailJob MapFromService(MatchyBackEnd.DetailJob detailJob)
        {
            return new DetailJob() {Data = detailJob.Data, DetailJobId = detailJob.DetailJobId, JobId = detailJob.JobId};
        }
    }
}
