using System;
using System.Collections.Generic;
using PythonInterpreter.Grammar;
using System.Linq;
using PythonInterpreter.Visitors;

namespace PythonInterpreter
{
    public static class PrintFunctionContextExtensions
    {
        public static void Evaluate(this PythonInterpreterParser.Print_funcContext context, Scope scope)
        {
            var arguments = context.arguments().GetText().Split(',');
            arguments = arguments.Select(x => scope.IntegerVariables.TryGetValue(x, out int value) ? value.ToString() : x).ToArray();
            var result = string.Join(' ', arguments).Trim(' ');
            Console.WriteLine(result);
        }
    }


}
