using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime.Misc;
using PythonInterpreter.Grammar;
using System.Linq;

namespace PythonInterpreter
{
    class Interpreter: PythonInterpreterBaseVisitor<int>
    {
        private Dictionary<string, int> IntegerVariables = new Dictionary<string, int>();

        public override int VisitAssignment_statement([NotNull] PythonInterpreterParser.Assignment_statementContext context)
        {
            var name = context.ID().GetText();
            var value = int.Parse(context.expression().INT().GetText());
            IntegerVariables[name] = value;
            return base.VisitAssignment_statement(context);
        }
        public override int VisitPrint_func([NotNull] PythonInterpreterParser.Print_funcContext context)
        {
            var arguments = context.arguments().GetText().Split(',');
            arguments = arguments.Select(x => IntegerVariables.TryGetValue(x, out int value) ? value.ToString() : x).ToArray();
            Console.WriteLine(string.Join(' ', arguments));
            return base.VisitPrint_func(context);
        }

        public override int VisitMin_func([NotNull] PythonInterpreterParser.Min_funcContext context)
        {
            var arguments = context.arguments().GetText().Split(',');
            var numbers = arguments.Select(x => GetIntValue(x));
            Console.WriteLine(numbers.Min());
            return base.VisitMin_func(context);
        }

        public override int VisitMax_func([NotNull] PythonInterpreterParser.Max_funcContext context)
        {
            var arguments = context.arguments().GetText().Split(',');
            var numbers = arguments.Select(x => GetIntValue(x));
            Console.WriteLine(numbers.Max());
            return base.VisitMax_func(context);
        }


        private int GetIntValue(string x) => IntegerVariables.TryGetValue(x, out int value) ? value : int.Parse(x);
    }
}
