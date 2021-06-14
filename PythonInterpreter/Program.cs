using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using PythonInterpreter.Grammar;
using CommandLine;

namespace PythonInterpreter
{
    class Options
    {
        [Option('f', "file", Required = false, HelpText = "File name to run an interpreter")]
        public string File { get; set; }

        [Option('o', "overexcited", Required = false, HelpText = "Should interpreter ask you every time to specify program file")]
        public bool Overexcited { get; set; }
    }
    class Program
    {
        public static string PROMPT => ">>";
        static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<Options>(args)
                       .WithParsed(o =>
                       {
                           var interpreter = new Interpreter();
                           if (o.Overexcited)
                           {
                               RunInOverexcitedMode(interpreter);
                           }
                           else if (!string.IsNullOrEmpty(o.File))
                           {
                               RunInterpreterOnFile(interpreter, o.File);
                           } 
                           else
                           {
                               RunInInteractiveConsole(interpreter);
                           }         
                       });
        }



        public static void RunInOverexcitedMode(Interpreter interpreter)
        {
            var isFile = false;
            var input = string.Empty;
            while (true)
            {
                isFile = MenuHelper.InputType();
                if (isFile)
                    input = MenuHelper.GetFile();
                else
                {
                    Console.Write(PROMPT);
                    input = Console.ReadLine();
                }
                RunInterprerOnInput(interpreter, input);

            }
        }

        public static void RunInterpreterOnFile(Interpreter interpreter, string fileName)
        {
            var input = MenuHelper.GetFileContents(fileName);
            RunInterprerOnInput(interpreter, input);
            Console.WriteLine("Execution endend. Press any key to continue ...");
            Console.Read();
        }

        public static void RunInInteractiveConsole(Interpreter interpreter)
        {
            while (true)
            {
                Console.Write(PROMPT);
                var input = Console.ReadLine();
                RunInterprerOnInput(interpreter, input);
            }
        }

        public static void RunInterprerOnInput(Interpreter interpreter, string input)
        {
            var stream = new AntlrInputStream(input);
            var lexer = new PythonInterpreterLexer(stream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new PythonInterpreterParser(tokens);
            var programParseTree = parser.program();
            interpreter.Visit(programParseTree);
        }
    }
}
