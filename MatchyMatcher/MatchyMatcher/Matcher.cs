using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessObjects;

namespace MatchyMatcher
{
    public class Matcher
    {

        //adjustable scores and score requirements
        private double minScore = 1.0;
        private double educationScore = 1;
        private double cityScore = 0.5;
        private double hourScore = 0.5;

        //Call this method to start matching the two objects
        public void StartMatching(Cv cv, Job job, DetailJob djob)
        {

            Console.WriteLine("Cv en Job vergelijken...");

            //create arrays with words to match
            string[] cvArray = new StringFilter().CleanUp(cv.EducationHistory + cv.Discipline + cv.Profession + cv.WorkExperience + cv.Interests + cv.Personal + cv.JobRequirements);
            string[] jobArray = new StringFilter().CleanUp(job.JobDescription + job.JobTitle + djob.Data);

            //match individual elements
            double WordScore = StartWordAnalyzer(cvArray, jobArray);
            double CityScore = StartCityAnalyzer(cv.City, job.Company.CompanyCity);
            double EducationScore = StartEducationAnalyzer(cv.EducationLevel, job.Education);
            double HourScore = StartHourMatcher(cv.Hours, job.JobHours);

            //calculate total score
            double MatchScore = WordScore + CityScore + EducationScore;

            Console.WriteLine("Vergelijken klaar!");

            //if MatchScore is higher then minScore
            if (MatchScore > minScore)
            {

                Console.WriteLine("Score is: " + MatchScore.ToString());
                AddMatch(cv, job, MatchScore);

            }
            else
            {
                Console.WriteLine("Geen match ):");
            }

        }

        //analyze words in both objects returns score according to the words matcher can be anything between 0-3
        private double StartWordAnalyzer(string[] cv, string[] job)
        {

            return new WordAnalyzer().Analyze(cv, job);

        }

        //analyze city return cityScore if the match
        private double StartCityAnalyzer(string cvCity, string jobCity)
        {

            if (!cvCity.Equals("Niet Beschikbaar") || !jobCity.Equals("Niet Beschikbaar"))
            {
                if (cvCity.Equals(jobCity))
                    return cityScore;
            }

            return 0.0;

        }

        //analyze education return educationScore if they match
        private double StartEducationAnalyzer(Education cvEdu, Education jobEdu)
        {

            if (cvEdu.EducationId == jobEdu.EducationId)
                return educationScore;

            return 0.0;

        }

        private double StartHourMatcher(string cvHours, string jobHours)
        {

            if (cvHours.Equals("Vast") || cvHours.Equals("Tijdelijk"))
            {

                if (jobHours.Contains("40"))
                    return hourScore;

            }
            else if (cvHours.Equals("Parttime"))
            {

                if (!jobHours.Contains("40"))
                    return hourScore;

            }

            return 0.1;

        }

        //add match to webservice
        private void AddMatch(Cv cv, Job job, double matchScore)
        {

            //create match
            Match match = new Match();

            //if score is above 5 make it 5
            if (matchScore > 5)
                matchScore = 5.0;

            //fill in match
            match.Cv = cv;
            match.Job = job;
            match.Score = Convert.ToInt32(Math.Round(matchScore, 0, MidpointRounding.AwayFromZero));

            //toevoegen aan webservice hier
            MatchyMapper matchmap = new MatchyMapper();
            Sender send = new Sender();

            send.addMatch(matchmap.MapToService(match));

        }

    }
}
