using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MLManager
{
    public static class TextAnalysisHelper
    {
        /**
            This class will implement all the text manipulations using regex & other methods
        **/


            /*
                Remove new lines frm the text and then spliting by dot
              */

        public static void  Analyze(string txt)
        {

        }


        public static ICollection<string> SplitByDot(string txt)
        {

            ICollection<string> ans = null;
            if (!string.IsNullOrEmpty(txt))
            {
                txt.Replace("\n", string.Empty);
                ans = txt.Split('.');
            }
            return ans;
        }

        /*
            Spliting the words using regex and counting them
        */
        public static Dictionary<string,int> CountWords(string txt)
        {
            Dictionary<string, int> wordsDic = null;
            if (!string.IsNullOrEmpty(txt))
            {
                Regex wordsRegex = new Regex(@"\w+");
                MatchCollection matchesList = wordsRegex.Matches(txt);
                if(matchesList!=null && matchesList.Count > 0)
                {
                    wordsDic = new Dictionary<string, int>();
                    foreach( Match match in matchesList)
                    {
                        var word = match.Value;
                        if (!string.IsNullOrEmpty(word))
                        {
                            if (wordsDic.ContainsKey(word))
                            {
                                wordsDic[word]++;
                                continue;
                            }
                            wordsDic.Add(word, 1);
                        }
                    }
                }
            }
            return wordsDic;
        }

        

    }
}
