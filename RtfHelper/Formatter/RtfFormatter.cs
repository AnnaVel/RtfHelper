using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RtfHelper.Formatter
{
    internal class RtfFormatter
    {
        public string GetFormattedText(string allText)
        {
            StringBuilder newText = new StringBuilder(allText.Length);

            newText.Append(allText[0]);

            for (int i = 1; i < allText.Length; i++)
            {
                if (allText[i] == '{')
                {
                    newText.AppendLine();
                }

                if (Environment.NewLine.All(c => allText[i] != c))
                {
                    newText.Append(allText[i]);
                }
            }

            return newText.ToString();
        }
    }
}
