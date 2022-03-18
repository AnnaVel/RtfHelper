using Kbg.NppPluginNET.PluginInfrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kbg.NppPluginNET
{
    public static class Extensions
    {
        public static string GetAllText(this ScintillaGateway scintillaGateway)
        {
            int length = scintillaGateway.GetLength();

            int textStride = 10000;
            int steps = (int)Math.Ceiling((double)length / textStride);
            string allText = string.Empty;

            for (int i = 0; i < steps; i++)
            {
                int startIndex = i * textStride;
                int endIndex = startIndex + textStride - 1;

                if (endIndex >= length)
                {
                    endIndex = length;
                }

                scintillaGateway.SetTargetRange(startIndex, endIndex);
                allText += scintillaGateway.GetTargetText();
            }

            return allText;
        }
    }
}
