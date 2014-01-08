using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessObjects;
using MatchyMatcher.MatchyBackend;

namespace MatchyMatcher
{
    class Retriever
    {

        MatchyService Backend = new MatchyService();

        public DataAccessObjects.Cv[] retrieveCv()
        {

            CvMapping cvmap = new CvMapping();

            MatchyBackend.Cv[] unprocessedCv = Backend.GetCv(0);
            DataAccessObjects.Cv[] processedCv = new DataAccessObjects.Cv[unprocessedCv.Length];

            for (int x = 0; x < unprocessedCv.Length; x++)
                processedCv[x] = cvmap.mapFromService(unprocessedCv[x]);

            return processedCv;

        }

        public DataAccessObjects.Job[] retrieveJob()
        {

            JobMapping jobmap = new JobMapping();

            MatchyBackend.Job[] unprocessedJob = Backend.GetJob(0);
            DataAccessObjects.Job[] processedJob = new DataAccessObjects.Job[unprocessedJob.Length];

            for (int x = 0; x < unprocessedJob.Length; x++)
                processedJob[x] = jobmap.mapFromService(unprocessedJob[x]);

            return processedJob;

        }

    }
}
