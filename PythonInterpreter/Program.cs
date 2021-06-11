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
            var isFile = false;
            var input = string.Empty;

            while (true)
            {
                isFile = MenuHelper.InputType();
                if (isFile)
                    input = MenuHelper.GetFile();
                else
                {
                    Console.Write(">>");
                    input = Console.ReadLine();
                }

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
