using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace MatchyMatcher
{
    public class StringFilter
    {

        //filter string used in this class only
        private string filter;

        //Whenever StringFi;lter is created create the filter list according to the file
        public StringFilter()
        {

            //read lines from txt document and put in array
            string[] filterWords = new string[] { "de", "een", "het", "zoals", "bijvoorbeeld", "onder", "andere", "waarom", "ik", "u", "ons", "bedrijf", "mijn" };//System.IO.File.ReadAllLines(@"C:\filter\filter_words.txt");



            //start creating the filter regex
            filter = "\\b(";

            //Keep adding thw words till all words are added
            for (int x = 0; x < filterWords.Length; x++ )
            {

                //first word do not add a | infront of it
                if(x == 0){
                    filter = filter + filterWords[x];
                }else{
                    filter = filter + "|" + filterWords[x];
                }
                
            }

            //Finish creating the string
            filter =  filter + ")\\b";

        }

        //cleans up any string according to the list in filterWords returns it clean in an array
        public string[] CleanUp(string junk)
        {

            //apply regex to filter words, trim whatever remains then split into individual strings in an array
            string clean = Regex.Replace(junk, filter, "", RegexOptions.IgnoreCase);
            clean = clean.Trim();
            string[] words = clean.Split(null);

            return words;

        }

    }
}
