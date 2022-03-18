using Microsoft.VisualStudio.TestTools.UnitTesting;
using RtfHelper.Formatter;
using System.IO;

namespace RtfHelperTests
{
    [TestClass]
    public class FormatterTests
    {
        private string shortRtfFormatted = @"{\rtf\ansi\ansicpg1252\uc1\deff0\deflang1033
{\fonttbl
{\f0 Verdana;}
{\f1 Symbol;}}
{\pard Urbem Romam a principio reges habuere; libertatem et consulatum L. Brutus instituit. dictaturae ad tempus sumebantur; neque decemviralis potestas ultra biennium, neque tribunorum militum consulare ius diu valuit. \sa200 \par}
{\pard Second paragraph. \par}}";

        private string rtfParWithoutBracketsFormatted = @"{\rtf\ansi\ansicpg1252\uc1\deff0\deflang1033
{\fonttbl
{\f0 Verdana;}
{\f1 Symbol;}}
\pard First paragraph \sa200 \par
{\pard Second paragraph. \par}}";

        [TestMethod]
        public void BasicScenario()
        {
            RtfFormatter formatter = new RtfFormatter();

            string input = @"{\rtf\ansi\ansicpg1252\uc1\deff0\deflang1033{\fonttbl{\f0 Verdana;}{\f1 Symbol;}}{\pard Urbem Romam a principio reges habuere; libertatem et consulatum L. Brutus instituit. dictaturae ad tempus sumebantur; neque decemviralis potestas ultra biennium, neque tribunorum militum consulare ius diu valuit. \sa200 \par}{\pard Second paragraph. \par}}";
            string expectedOutput = shortRtfFormatted;

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
            string expectedOutput = shortRtfFormatted;

            string output = formatter.GetFormattedText(input);

            Assert.AreEqual(expectedOutput, output);
        }

        [TestMethod]
        public void TextAlreadyFormatted()
        {
            RtfFormatter formatter = new RtfFormatter();

            string input = shortRtfFormatted;
            string expectedOutput = shortRtfFormatted;

            string output = formatter.GetFormattedText(input);

            Assert.AreEqual(expectedOutput, output);
        }

        [TestMethod]
        public void UnnecessaryNewLinesAreRemoved()
        {
            RtfFormatter formatter = new RtfFormatter();

            string input = @"{\rtf\ansi\ansicpg1252\uc1\deff0\deflang1033
{\fonttbl{\f0 Verdana;}{\f1 Symbol;}

}


{\pard Urbem R
omam a p
rincipio r
eges habuere; libertatem et consulatum L. Brutus instituit. dictaturae ad tempus sumebantur; neque decemviralis potestas ultra biennium, neque tribunorum militum consulare ius diu valuit. \sa200 \par}{\pard Second paragraph. \par}}";
            string expectedOutput = shortRtfFormatted;

            string output = formatter.GetFormattedText(input);

            Assert.AreEqual(expectedOutput, output);
        }

        [TestMethod]
        public void TextContainsBlocksWithoutCurlyBrackets()
        {
            RtfFormatter formatter = new RtfFormatter();

            string input = @"{\rtf\ansi\ansicpg1252\uc1\deff0\deflang1033{\fonttbl{\f0 Verdana;}{\f1 Symbol;}}\pard First paragraph \sa200 \par{\pard Second paragraph. \par}}";
            string expectedOutput = this.rtfParWithoutBracketsFormatted;

            string output = formatter.GetFormattedText(input);

            Assert.AreEqual(expectedOutput, output);
        }

        [TestMethod]
        public void TextPartiallyFormattedWithNewLineBeforeBlockWithoutBrackets()
        {
            RtfFormatter formatter = new RtfFormatter();

            string input = @"{\rtf\ansi\ansicpg1252\uc1\deff0\deflang1033{\fonttbl{\f0 Verdana;}{\f1 Symbol;}}
\pard First paragraph \sa200 \par{\pard Second paragraph. \par}}";
            string expectedOutput = rtfParWithoutBracketsFormatted;

            string output = formatter.GetFormattedText(input);

            Assert.AreEqual(expectedOutput, output);
        }

        [TestMethod]
        public void TextContainsCommandSeparatedWithNewLine()
        {
            RtfFormatter formatter = new RtfFormatter();

            string input = @"{\rtf\ansi\ansicpg1252\uc1\deff0\deflang1033{\fonttbl{\f0 Verdana;}{\f1 Symbol;}}{\pard Urbem Romam a principio reges habuere; libertatem et consulatum L. Brutus instituit. dictaturae ad tempus sumebantur; neque decemviralis potestas ultra biennium, neque tribunorum militum consulare ius diu valuit. \sa200
\par}{\pard Second paragraph. \par}}";
            string expectedOutput = shortRtfFormatted;

            string output = formatter.GetFormattedText(input);

            Assert.AreEqual(expectedOutput, output);
        }

        [TestMethod]
        public void FormatTwiceBasicScenario()
        {
            RtfFormatter formatter = new RtfFormatter();

            string input = @"{\rtf\ansi\ansicpg1252\uc1\deff0\deflang1033{\fonttbl{\f0 Verdana;}{\f1 Symbol;}}{\pard Urbem Romam a principio reges habuere; libertatem et consulatum L. Brutus instituit. dictaturae ad tempus sumebantur; neque decemviralis potestas ultra biennium, neque tribunorum militum consulare ius diu valuit. \sa200 \par}{\pard Second paragraph. \par}}";
            string expectedOutput = shortRtfFormatted;

            string output = formatter.GetFormattedText(input);

            Assert.AreEqual(expectedOutput, output);

            output = formatter.GetFormattedText(input);

            Assert.AreEqual(expectedOutput, output);
        }

        [TestMethod]
        public void EscapedBracketsAreNotTreatedAsBlockBegining()
        {
            RtfFormatter formatter = new RtfFormatter();

            string input = @"{\rtf{\pard\listtext \{0\}.}\par}}";
            string expectedOutput = @"{\rtf
{\pard\listtext \{0\}.}
\par}}";

            string output = formatter.GetFormattedText(input);

            Assert.AreEqual(expectedOutput, output);

            output = formatter.GetFormattedText(input);

            Assert.AreEqual(expectedOutput, output);
        }

        [TestMethod]
        public void NonRtfText()
        {
            RtfFormatter formatter = new RtfFormatter();

            string input = @"fsdfd";
            string output = formatter.GetFormattedText(input);
        }

        [TestMethod]
        public void LongRtf()
        {
            RtfFormatter formatter = new RtfFormatter();

            Stream stream = File.OpenRead(@"..\..\..\LongRtf.txt");
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
