using System;
using System.Collections.Generic;
using PythonInterpreter.Grammar;


namespace PythonInterpreter
{
    public static class ConditionContextExtensions
    {
        public static bool IsTrue(this PythonInterpreterParser.ConditionContext context, Dictionary<string, int> variables)
        {
            
            if (context.expression().Length == 1)
            {
                return context.expression()[0].Evaluate(variables) != 0;
            }
            var op = context.COMPARISON_OPERATOR().GetText();
            var left = context.expression()[0].Evaluate(variables);
            var right = context.expression()[1].Evaluate(variables);
            return op switch {
                "==" => left == right,
                "!=" => left != right,
                "<=" => left <= right,
                ">=" => left >= right,
                ">" => left > right,
                "<" => right < left,
                _ => throw new Exception("Not allowed operator")
            };
        }
    }


}
