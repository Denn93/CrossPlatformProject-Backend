using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessObjects;

namespace MatchyMatcher
{
    class Program
    {
        static void Main(string[] args)
        {

            Matcher matcher = new Matcher();
            Retriever retriever = new Retriever();

            Cv[] cv = retriever.retrieveCv();
            Job[] job = retriever.retrieveJob();

            for (int x = 0; x < cv.Length; x++ )
            {

                for (int y = 0; y < job.Length; y++)
                {

                    matcher.StartMatching(cv[x], job[y], job[y].DetailJob);

                }

            }

        }
    }
}
 