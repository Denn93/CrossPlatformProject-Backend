using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatchyMatcher.MatchyBackend;

namespace MatchyMatcher
{
    class Sender
    {

        MatchyService Backend = new MatchyService();

        public void addMatch(Match match){

            Backend.AddMatch(match);

        }

    }
}
