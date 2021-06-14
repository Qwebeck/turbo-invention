using System;
using PythonInterpreter.Visitors;
using PythonInterpreter.Extensions;
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
                           var interpreter = new MainVisitor();
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



        public static void RunInOverexcitedMode(MainVisitor interpreter)
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

        public static void RunInterpreterOnFile(MainVisitor interpreter, string fileName)
        {
            var input = MenuHelper.GetFileContents(fileName);
            RunInterprerOnInput(interpreter, input);
            Console.WriteLine("Execution endend. Press any key to continue ...");
            Console.Read();
        }

        public static void RunInInteractiveConsole(MainVisitor interpreter)
        {
            while (true)
            {
                Console.Write(PROMPT);
                var input = Console.ReadLine();
                RunInterprerOnInput(interpreter, input);
            }
        }

        public static void RunInterprerOnInput(MainVisitor interpreter, string input)
        {
            var result = interpreter.RunOnInput(input);
            if (result != MainVisitor.NONE)
            {
                Console.WriteLine(result);
            }
        }
    }
}
