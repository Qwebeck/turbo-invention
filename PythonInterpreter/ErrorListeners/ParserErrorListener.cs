using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;

namespace PythonInterpreter.Extensions
{

    public class BadParserInputException: Exception
    {
        public BadParserInputException(string msg) : base(msg) { }
    }
    public class ParserErrorListener : IAntlrErrorListener<IToken>
    {
        public void SyntaxError([NotNull] IRecognizer recognizer, 
            [Nullable] IToken offendingSymbol, int line, int charPositionInLine, 
            [NotNull] string msg, [Nullable] RecognitionException e)
        {
            throw new BadParserInputException(
                    $"At line {line} at position {charPositionInLine}: {msg}"
                );
        }
    }
}
