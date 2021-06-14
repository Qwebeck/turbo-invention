using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;

namespace PythonInterpreter.Extensions
{
    public class BadLexerInputException : Exception
    {
        public BadLexerInputException(string msg) : base(msg) { }
    }
    public class LexerErrorListener : IAntlrErrorListener<int>
    {
        public void SyntaxError([NotNull] IRecognizer recognizer, [Nullable] int offendingSymbol, 
            int line, int charPositionInLine, [NotNull] string msg, [Nullable] RecognitionException e)
        {
            throw new BadLexerInputException(
                   $"At line {line} at position {charPositionInLine}: {msg}"
               );
        }
    }
}
