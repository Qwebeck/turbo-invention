using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using PythonInterpreter.Grammar;

namespace PythonInterpreter
{
    class Program
    {
        static void Main()
        {
            var interpreter = new Interpreter();
            while (true)
            {
                Console.Write(">>");
                var input = Console.ReadLine();
                var stream = new AntlrInputStream(input);
                var lexer = new PythonInterpreterLexer(stream);
                var tokens = new CommonTokenStream(lexer);
                var parser = new PythonInterpreterParser(tokens);
                var programParseTree = parser.program();
                interpreter.Visit(programParseTree);
            }
           
        }
    }
}
