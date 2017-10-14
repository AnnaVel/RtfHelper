using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RtfHelper.Formatter;
using System.IO;

namespace RtfHelperTests
{
    [TestClass]
    public class FormatterTests
    {
        private string shortRtfExpectedOutput = @"{\rtf\ansi\ansicpg1252\uc1\deff0\deflang1033
{\fonttbl
{\f0 Verdana;}
{\f1 Symbol;}}
{\pard Urbem Romam a principio reges habuere; libertatem et consulatum L. Brutus instituit. dictaturae ad tempus sumebantur; neque decemviralis potestas ultra biennium, neque tribunorum militum consulare ius diu valuit. \sa200 \par}
{\pard Second paragraph. \par}}";

        [TestMethod]
        public void BasicScenario()
        {
            RtfFormatter formatter = new RtfFormatter();

            string input = @"{\rtf\ansi\ansicpg1252\uc1\deff0\deflang1033{\fonttbl{\f0 Verdana;}{\f1 Symbol;}}{\pard Urbem Romam a principio reges habuere; libertatem et consulatum L. Brutus instituit. dictaturae ad tempus sumebantur; neque decemviralis potestas ultra biennium, neque tribunorum militum consulare ius diu valuit. \sa200 \par}{\pard Second paragraph. \par}}";
            string expectedOutput = shortRtfExpectedOutput;

            string output = formatter.GetFormattedText(input);

            Assert.AreEqual(expectedOutput, output);
        }

        [TestMethod]
        public void TextAlreadyPartiallyFormatted()
        {
            RtfFormatter formatter = new RtfFormatter();

            string input = @"{\rtf\ansi\ansicpg1252\uc1\deff0\deflang1033
{\fonttbl{\f0 Verdana;}{\f1 Symbol;}}
{\pard Urbem Romam a principio reges habuere; libertatem et consulatum L. Brutus instituit. dictaturae ad tempus sumebantur; neque decemviralis potestas ultra biennium, neque tribunorum militum consulare ius diu valuit. \sa200 \par}{\pard Second paragraph. \par}}";
            string expectedOutput = shortRtfExpectedOutput;

            string output = formatter.GetFormattedText(input);

            Assert.AreEqual(expectedOutput, output);
        }

        [TestMethod]
        public void LongRtf()
        {
            RtfFormatter formatter = new RtfFormatter();

            Stream stream = File.OpenRead(@"..\..\LongRtf.txt");
            string input = "";

            using (stream)
            {
                StreamReader reader = new StreamReader(stream);
                input = reader.ReadToEnd();
            }

            string output = formatter.GetFormattedText(input);
        }
    }
}
