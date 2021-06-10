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
        private readonly Dictionary<string, int> IntegerVariables = new Dictionary<string, int>();

        public override int VisitAssignment_statement([NotNull] PythonInterpreterParser.Assignment_statementContext context)
        {
            var name = context.ID().GetText();
            var value = int.Parse(context.expression().INT().GetText());
            IntegerVariables[name] = value;
            return base.VisitAssignment_statement(context);
        }
        public override int VisitPrint_func([NotNull] PythonInterpreterParser.Print_funcContext context)
        {
            context.Evaluate(IntegerVariables);
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


        public override int VisitIf_statement([NotNull] PythonInterpreterParser.If_statementContext context)
        {
            var condition = context.condition();
            if (condition.IsTrue(IntegerVariables))
            {
                var statement = context.statement_list()[0];
                return base.VisitStatement_list(statement);
            }
            else if (context.ELSE() != null)
            {
                var statement = context.statement_list()[1];
                return base.VisitStatement_list(statement);
            }
            return 0;
        }

        private int GetIntValue(string x) => IntegerVariables.TryGetValue(x, out int value) ? value : int.Parse(x);
    }

    
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


    public static class StatementListContextExtensions
    {
        public static int Evaluate(this PythonInterpreterParser.Statement_listContext context, Dictionary<string, int> variables)
        {
            context.statement()[0].function_call_statement().library_func().print_func().Evaluate(variables);
            return 0;
        }
    }

    public static class PrintFunctionContextExtensions
    {
        public static void Evaluate(this PythonInterpreterParser.Print_funcContext context, Dictionary<string, int> variables)
        {
            var arguments = context.arguments().GetText().Split(',');
            arguments = arguments.Select(x => variables.TryGetValue(x, out int value) ? value.ToString() : x).ToArray();
            Console.WriteLine(string.Join(' ', arguments));
        }
    }


}
