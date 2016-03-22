//using NHunspell;
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




        public static void  Analyze(string txt)
        {

        }

        /*
             Remove new lines from the text and then spliting by dot
        */
        public static List<string> SplitByDot(string txt)
        {

            List<string> ans = null;
            if (!string.IsNullOrEmpty(txt))
            {
                txt.Replace("\n", string.Empty);
                ans = txt.Split('.').ToList();
                for (int i = 0; i < ans.Count; i++)
                {
                    ans[i] = ans[i].Trim();
                }

                while (ans.Contains(""))
                {
                    ans.Remove("");
                }
            }
            return ans;
        }

        /*
            Counting each word
        */
        public static Dictionary<string, int> CountWords(List<List<string>> WordsPerSentenceList)
        {
            Dictionary<string, int> wordsDic = new Dictionary<string, int>();
            foreach (var sentence in WordsPerSentenceList)
            {
                foreach (var word in sentence)
                {
                    if (!wordsDic.ContainsKey(word))
                    {
                        wordsDic.Add(word, 1);
                    }
                    else
                    {
                        wordsDic[word]++;
                    }
                }
            }

            return wordsDic;
        }



        /* 
             Split the text by new line 
        */
        public static List<string> SplitByNewLine(string text)
        {
            List<string> ans = null;
            if (!string.IsNullOrEmpty(text))
            {
                ans = new List<string>(text.Split('\n'));

                for (int i = 0; i < ans.Count; i++)
                {
                    ans[i] = ans[i].Trim();
                }

                while (ans.Contains(""))
                {
                    ans.Remove("");
                }
            }

            return ans;
        }

        public static List<List<string>> WordsPerSentence(List<string> SentenceList)
        {
            List<List<string>> WordsList = new List<List<string>>();
            foreach (string sentence in SentenceList)
            {
                List<string> temp = new List<string>(sentence.Split(' '));
                WordsList.Add(temp);
            }

            for (int i = 0; i < WordsList.Count; i++)
            {
                for (int j = 0; j < WordsList[i].Count; j++)
                {
                    Regex Reg = new Regex(@"[\w\d,.;:]+");
                    Match Mat = Reg.Match(WordsList[i][j]);
                    if (Mat != null && Mat.Success)
                    {
                        WordsList[i][j] = Mat.Value.ToString();
                    }
                    while (WordsList[i].Contains(""))
                    {
                        WordsList[i].Remove("");
                    }

                }
            }
            return WordsList;
        }

        public static double GetSentenceAVG(List<List<string>> WordsPerSentenceList)
        {
            double sum = 0;
            foreach (var sentence in WordsPerSentenceList)
            {
                sum += sentence.Count;
            }

            return sum / WordsPerSentenceList.Count;
        }

        public static double GetWordAVG(Dictionary<string, int> WordsDict)
        {
            double CharCount = 0;
            double TotalWordsCount = 0;
            foreach (var word in WordsDict.Keys)
            {
                CharCount += word.Length * WordsDict[word];
                TotalWordsCount += WordsDict[word];
            }

            return CharCount / TotalWordsCount;
        }

        public static double GetTokenRatio(Dictionary<string, int> WordsDict)
        {
            double NumberOfKeys = WordsDict.Keys.Count;
            double TotalWordsCount = 0;
            foreach (var word in WordsDict.Keys)
            {
                TotalWordsCount += WordsDict[word];
            }

            return NumberOfKeys / TotalWordsCount;
        }


       /* public static bool CheckSpell(string word)
        {
            // todo : replace Hunspell with 'How to: Use COM Interop to Check Spelling Using Word (C#)'
            using (Hunspell hunspell = new Hunspell("en_GB.aff", "en_GB.dic"))
            {
                return hunspell.Spell(word);
            }
        }*/
    }


    
}
