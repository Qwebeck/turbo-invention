using System.Collections.Generic;
using PythonInterpreter.Grammar;
using System.Linq;
using PythonInterpreter.Visitors;

namespace PythonInterpreter
{
    public static class ExpressionContextExtensions
    {
        
        public static int Evaluate(this PythonInterpreterParser.ExpressionContext context, Scope scope)
        {
            // Work with different types of contexts
            var x = context.GetText();
            return scope.IntegerVariables.TryGetValue(x, out int value) ? value : int.Parse(x);
        }

        public static int Evaluate(this PythonInterpreterParser.ExpressionContext[] context, Scope scope)
        { 
            return context.Sum(x => x.Evaluate(scope));
        }
    }


}
