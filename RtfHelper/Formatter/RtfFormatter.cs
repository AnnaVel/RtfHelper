using System;
using System.Linq;
using System.Text;

namespace RtfHelper.Formatter
{
    public class RtfFormatter
    {
        private Action<StringBuilder, string> processNext;

        private int index;

        public RtfFormatter()
        {
            this.processNext = this.ProcessText;
        }

        public string GetFormattedText(string input)
        {
            StringBuilder output = new StringBuilder();
            this.ResetFormatter();

            while (this.index < input.Length)
            {
                this.processNext(output, input);
            }

            return output.ToString();
        }

        private void ResetFormatter()
        {
            this.processNext = this.ProcessText;
            this.index = 0;
        }

        private void ProcessText(StringBuilder output, string input)
        {
            int currentIndex = this.index;

            this.AppendAndMove(output, input[this.index]);

            bool nextCharIsOpeningBracket = this.PeekNext(input, currentIndex) == '{';
            bool thisCharIsClosingBracket = input[currentIndex] == '}';
            bool nextCharIsClosingBracket = this.PeekNext(input, currentIndex) == '}';
            bool nextCharIsNewLine = this.CharIsNewLine(this.PeekNext(input, currentIndex));
            bool thisCharIsPartOfCommand = this.CharIsPartOfRtfCommand(input, currentIndex);
            
            if (nextCharIsOpeningBracket)
            {
                this.processNext = this.ProcessBlockStart;
            }
            else if (nextCharIsNewLine && thisCharIsPartOfCommand)
            {
                this.processNext = this.ProcessNewLineAfterCommandCharacter;
            }
            else if(nextCharIsNewLine)
            {
                this.processNext = this.ProcessNewLine;
            }
            else if(thisCharIsClosingBracket && !nextCharIsClosingBracket)
            {
                this.processNext = this.ProcessBlockStart;
            }
        }

        private void ProcessBlockStart(StringBuilder output, string input)
        {
            output.AppendLine();

            this.processNext = this.ProcessText;
        }

        private void ProcessNewLine(StringBuilder output, string input)
        {
            this.index += Environment.NewLine.Length;
            int currentIndex = this.index - 1; // Positioning the current index behind the next, that is, at the end of the new line.

            bool nextCharIsOpeningBracket = this.PeekNext(input, currentIndex) == '{';
            bool previousMeaningfulCharacterIsClosingBracket = this.LookAtPreviousSkippingNewLines(input, currentIndex) == '}';
            bool nextCharIsClosingBracket = this.PeekNext(input, currentIndex) == '}';
            bool nextCharIsNewLine = this.CharIsNewLine(this.PeekNext(input, currentIndex));

            if (nextCharIsOpeningBracket || 
                (previousMeaningfulCharacterIsClosingBracket &&
                !nextCharIsClosingBracket && !nextCharIsNewLine))
            {
                this.processNext = this.ProcessBlockStart;
            }
            else if (!nextCharIsNewLine)
            {
                this.processNext = this.ProcessText;
            }
        }

        private void ProcessNewLineAfterCommandCharacter(StringBuilder output, string input)
        {
            this.index += Environment.NewLine.Length;
            int currentIndex = this.index - 1; // Positioning the current index behind the next, that is, at the end of the new line.

            bool nextCharIsNewLine = this.CharIsNewLine(this.PeekNext(input, index));
            bool nextCharIsOpeningBracket = this.PeekNext(input, currentIndex) == '{';

            bool nextHandlerIsProcessBlockStart = nextCharIsOpeningBracket;

            if (!nextHandlerIsProcessBlockStart)
            {
                output.Append(' ');
            }

            if (nextCharIsNewLine)
            {
                this.processNext = this.ProcessNewLine;
            }
            else if(nextHandlerIsProcessBlockStart)
            {
                this.processNext = this.ProcessBlockStart;
            }
            else
            {
                this.processNext = this.ProcessText;
            }
        }

        private void AppendAndMove(StringBuilder output, char character)
        {
            output.Append(character);
            this.index++;
        }

        private char? PeekNext(string input, int index)
        {
            if(index + 1 < input.Length)
            {
                return input[index + 1];
            }

            return null;
        }

        private char? LookAtPreviousSkippingNewLines(string input, int index)
        {
            while(index - 1 >= 0 &&
                this.CharIsNewLine(input[index - 1]))
            {
                index--;
            }

            return this.LookAtPrevious(input, index);
        }

        private char? LookAtPrevious(string input, int index)
        {
            if (index - 1 >= 0)
            {
                return input[index - 1];
            }

            return null;
        }

        private bool CharIsPartOfRtfCommand(string input, int currentIndex)
        {
            char currentChar = input[currentIndex];

            if(!this.IsAllowedInRtfCommand(currentChar))
            {
                return false;
            }

            currentIndex--;

            if (currentIndex <= 0)
            {
                return false;
            }

            currentChar = input[currentIndex];

            while(this.IsAllowedInRtfCommand(currentChar))
            {
                currentIndex--;

                if (currentIndex <= 0)
                {
                    return false;
                }

                currentChar = input[currentIndex];
            }

            return currentChar == '\\';
        }

        private bool IsAllowedInRtfCommand(char character)
        {
            return char.IsLetterOrDigit(character) &&
                (int)character >= 32 && (int)character <= 126;
        }

        private bool CharIsNewLine(char? character)
        {
            return Environment.NewLine.Any(c => character == c);
        }
    }
}
