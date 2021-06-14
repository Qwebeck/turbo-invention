using System.Collections.Generic;
using PythonInterpreter.Grammar;
using System.Linq;


namespace PythonInterpreter
{
    public static class ExpressionContextExtensions
    {
        
        public static int Evaluate(this PythonInterpreterParser.ExpressionContext context, Dictionary<string, int> variables)
        {
            // Work with different types of contexts
            var x = context.GetText();
            return variables.TryGetValue(x, out int value) ? value : int.Parse(x);
        }

        public static int Evaluate(this PythonInterpreterParser.ExpressionContext[] context, Dictionary<string, int> variables)
        { 
            return context.Sum(x => x.Evaluate(variables));
        }
    }


}
