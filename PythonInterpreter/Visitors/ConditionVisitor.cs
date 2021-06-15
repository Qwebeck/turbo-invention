using Antlr4.Runtime.Misc;
using PythonInterpreter.Grammar;
using System;
using System.Collections.Generic;
using System.Text;

namespace PythonInterpreter.Visitors
{
    class ConditionVisitor: PythonInterpreterBaseVisitor<bool>
    {
        private readonly Scope Scope;
        public ConditionVisitor(Scope scope) => Scope = scope;
        public override bool VisitExpressionCondition([NotNull] PythonInterpreterParser.ExpressionConditionContext context)
        {
            if (context.expression().Length == 1)
            {
                return context.expression()[0].Evaluate(Scope) != 0;
            }
            var op = context.COMPARISON_OPERATOR().GetText();
            var left = context.expression()[0].Evaluate(Scope);
            var right = context.expression()[1].Evaluate(Scope);
            return op switch
            {
                "==" => left == right,
                "!=" => left != right,
                "<=" => left <= right,
                ">=" => left >= right,
                ">" => left > right,
                "<" => right < left,
                _ => throw new Exception("Not allowed operator")
            };
        }

        public override bool VisitLogicalCondition([NotNull] PythonInterpreterParser.LogicalConditionContext context)
        {
            var isNegated = context.NOT() != null;
            var logicalValue = context.TRUE() != null ? true : false;
            return isNegated ? !logicalValue : logicalValue;
        }
    }
}
