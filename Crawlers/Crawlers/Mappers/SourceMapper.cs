using System;
using DataAccessObjects;

namespace CrawlerBatch.Mappers
{
    public class SourceMapper : AMapper<Source, MatchyBackEnd.Source>
    {
        public override int Insert(Source obj)
        {
            int insertId = MatchyBackend.AddSource(MapToService(obj));

            if (insertId == 0)
                Console.WriteLine("Source not added!!");

            return insertId;
        }

        public override Source[] Get(int id = 0)
        {
            MatchyBackEnd.Source[] sources = MatchyBackend.GetSource(0);

            Source[] resultlist = new Source[sources.Length];

            int i = 0;
            foreach (var source in sources)
            {
                Source result = MapFromService(source);

                resultlist[i] = result;
                i++;
            }

            return resultlist;
        }

        public override MatchyBackEnd.Source MapToService(Source source)
        {
            return new MatchyBackEnd.Source() { SourceId = source.SourceId, Description = source.Description };
        }

        public override Source MapFromService(MatchyBackEnd.Source source)
        {
            return new Source() {SourceId = source.SourceId, Description = source.Description};
        }
    }
}
