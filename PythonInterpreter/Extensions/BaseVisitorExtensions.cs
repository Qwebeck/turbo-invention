using Antlr4.Runtime;
using PythonInterpreter.Grammar;
using System;
using System.Collections.Generic;
using System.Text;

namespace PythonInterpreter.Extensions
{
    public static class BaseVisitorExtensions
    {
        public static T RunOnInput<T>(this PythonInterpreterBaseVisitor<T> visitor, string input)
        {
            var stream = new AntlrInputStream(input);
            var lexer = new PythonInterpreterLexer(stream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new PythonInterpreterParser(tokens);
            var programParseTree = parser.program();
            return visitor.Visit(programParseTree);
        }
    }
}
