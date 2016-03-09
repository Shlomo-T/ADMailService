using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLManager
{
    public static class TextAnalysisHelper
    {
        /**
            This class will implement all the text manipulations using regex & other methods
        **/

        public static List<string> TextToSentence(string text)
        {
            List<string> ans = null;
            if (text.Length > 0)
            {
                ans = new List<string>(text.Split('\n'));
                while (ans.Contains(""))
                {
                    ans.Remove("");
                }
            }

            return ans;
        }


    }
}
