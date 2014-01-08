using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace MatchyMatcher
{
    class WordAnalyzer
    {

        private string[] CvWords;
        private string[] JobWords;
        private int[] matchedWordsCountCv;
        private int[] matchedWordsCountJob;
        private double[] matchedWordsCountAvg;

        private ArrayList matchedWords = new ArrayList();

        private double totalScore;
        private double score;
        private double maxScore = 3.5;
        private double correctionScore = 1.3;

        public double Analyze(string[] CvWords, string[] JobWords)
        {

            this.CvWords = CvWords;
            this.JobWords = JobWords;

            if (CvWords.Length <= 1 || JobWords.Length <= 1)
                return 0.0;

            score = 0.0;

            GetMatchedWords();
            CountMatchedWords();
            CalculateScore();

            return score;

        }

        private void GetMatchedWords()
        {

            //Check if the words match if match add to arrayList if it is not in the arrayList already
            for (int x = 0; x < CvWords.Length; x++ )
            {
                for (int y = 0; y < JobWords.Length; y++)
                {
                    if (CvWords[x].Equals(JobWords[y]))
                    {
                        if (!matchedWords.Contains(JobWords[y]))
                        {
                            matchedWords.Add(JobWords[y]);
                        }
                    }
                }
            }

            matchedWordsCountCv = new int[matchedWords.Count];
            matchedWordsCountJob = new int[matchedWords.Count];
            matchedWordsCountAvg = new double[matchedWords.Count];

        }

        private void CountMatchedWords()
        {

            int counter = 0;

            //Count how many times a word is in CV
            foreach (string word in matchedWords)
            {
                for (int x = 0; x < CvWords.Length; x++)
                {
                    if (CvWords[x].Equals(word))
                    {
                        matchedWordsCountCv[counter]++;
                    }
                }
                counter++;
            }

            counter = 0;

            //Count how many times a word is in Job
            foreach (string word in matchedWords)
            {
                for (int x = 0; x < JobWords.Length; x++)
                {
                    if (JobWords[x].Equals(word))
                    {
                        matchedWordsCountJob[counter]++;
                    }
                }
                counter++;
            }

            //calculate average score each word
            for (int x = 0; x < matchedWordsCountAvg.Length; x++)
            {
                matchedWordsCountAvg[x] = ((double) matchedWordsCountCv[x] + (double) matchedWordsCountJob[x]) / 2;
            }

        }

        private void CalculateScore()
        {

            double AvgWordCount = ((double )CvWords.Length + (double) JobWords.Length) / 2;

            foreach (int i in matchedWordsCountAvg)
            {
                totalScore = totalScore + (double) i;
            }

            double tempScore = (double) totalScore / (double) AvgWordCount;
            score = ((tempScore * correctionScore) * maxScore);

            if (score > 3)
                score = 3;

        }

    }
}
