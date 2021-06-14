using System;
using System.Collections.Generic;
using PythonInterpreter.Grammar;
using System.Linq;


namespace PythonInterpreter
{
    public static class PrintFunctionContextExtensions
    {
        public static void Evaluate(this PythonInterpreterParser.Print_funcContext context, Dictionary<string, int> variables)
        {
            var arguments = context.arguments().GetText().Split(',');
            arguments = arguments.Select(x => variables.TryGetValue(x, out int value) ? value.ToString() : x).ToArray();
            var result = string.Join(' ', arguments).Trim(' ');
            Console.WriteLine(result);
        }
    }


}
