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

            int indentLevel = 0;
            string indent = "    ";

            for (int i = 0; i < allText.Length; i++)
            {
                if (allText[i] == '{')
                {
                    newText.AppendLine();

                    for (int u = 0; u < indentLevel; u++)
                    {
                        newText.Append(indent);
                    }

                    indentLevel++;
                }

                if (allText[i] == '}')
                {
                    indentLevel--;
                }

                newText.Append(allText[i]);
            }

            return newText.ToString();
        }
    }
}
