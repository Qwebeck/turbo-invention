using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime.Misc;
using PythonInterpreter.Grammar;
using System.Linq;


namespace PythonInterpreter.Visitors
{
    public class MainVisitor: PythonInterpreterBaseVisitor<int>
    {
        public static int ERROR_EXIT_CODE => 1;
        public static int SUCCESS_EXIT_CODE => 0;
        public static int NONE => int.MinValue; // TODO: make it smarter

        #region members 

        private Dictionary<string, int> IntegerVariables = new Dictionary<string, int>();


        private readonly Dictionary<string, FunctionDefinition> Functions = new Dictionary<string, FunctionDefinition>();
        #endregion

        #region function handling
 
        public override int VisitFunction_definition([NotNull] PythonInterpreterParser.Function_definitionContext context)
        {
            var signature = context.function_signature();
            var functionName = signature.ID().ToString();
            var functionParams = signature.parameters().ID().Select(id => id.GetText());
            // TODO fix to handle more sophisticated variables
            var functionDefinition = new FunctionDefinition(functionName, functionParams, context.function_body());
            Functions[functionName] = functionDefinition;
            return SUCCESS_EXIT_CODE;
        }

        public override int VisitFunction_call_statement([NotNull] PythonInterpreterParser.Function_call_statementContext context)
        {
            // TODO: library function call should be handled by grammar.
            var libraryFunction = context.library_func();
            if(libraryFunction != null)
            {
                return base.VisitLibrary_func(libraryFunction);
            }
            var functionName = context.ID().GetText();
            var arguments = context.arguments().GetText();
            var globalScope = IntegerVariables;
            var calledFunction = Functions[functionName];
            IntegerVariables = calledFunction.CreateLocalScope(arguments, globalScope);
            var result = base.VisitFunction_body(calledFunction.FunctionBody);
            IntegerVariables = globalScope;
            return result;
        }

        public override int VisitFunction_body([NotNull] PythonInterpreterParser.Function_bodyContext context)
        {
            var IsHasReturnStatement = context.RETURN() != null;
            return IsHasReturnStatement 
                ? base.VisitStatement_list(context.statement_list()) 
                : NONE;
        }
        #endregion

        #region Statements 

        public override int VisitAssignment_statement([NotNull] PythonInterpreterParser.Assignment_statementContext context)
        {
            var name = context.ID().GetText();
            var value = int.Parse(context.expression().INT().GetText());
            IntegerVariables[name] = value;
            return NONE;
        }

        public override int VisitIf_statement([NotNull] PythonInterpreterParser.If_statementContext context)
        {
            var validationErrors = context.Validate();
            if (validationErrors.Any())
            {
                validationErrors.ForEach(error =>
                {
                    Console.WriteLine(validationErrors);
                });
                return ERROR_EXIT_CODE;
            }
            var isConditionTrue = new ConditionVisitor(IntegerVariables).Visit(context.condition()[0]);
            var stamentLists = context.statement_list();
            if (isConditionTrue)
            {
                var statement = context.statement_list()[0];
                return base.Visit(statement);
            }
            else if (context.ELSEIF() != null && new ConditionVisitor(IntegerVariables).Visit(context.condition()[1]))
            {
                return base.Visit(stamentLists[1]);
            }
            else if (context.ELSE() != null)
            {
                var statement = stamentLists.Last();
                return base.Visit(statement);
            }
            return SUCCESS_EXIT_CODE;
        }



        #region Math
        public override int VisitAdditionStatement([NotNull] PythonInterpreterParser.AdditionStatementContext context)
        {
            var statement = context.math_statement();
            var left = statement[0];
            var right = statement[1];
            return context.PLUS() == null 
                ? base.Visit(left) - base.Visit(right)
                : base.Visit(left) + base.Visit(right);
        }

        public override int VisitMultiplicationStatement([NotNull] PythonInterpreterParser.MultiplicationStatementContext context)
        {
            var statement = context.math_statement();
            var left = statement[0];
            var right = statement[1];
            return context.TIMES() == null
                ? base.Visit(left) / base.Visit(right)
                : base.Visit(left) * base.Visit(right);
        }

        public override int VisitFactor([NotNull] PythonInterpreterParser.FactorContext context)
        {
            return int.Parse(context.INT().GetText());
        }

        public override int VisitParenthesedStatemet([NotNull] PythonInterpreterParser.ParenthesedStatemetContext context)
        {
            var statement = context.math_statement();
            return base.Visit(statement);
        }
        public override int VisitStatement([NotNull] PythonInterpreterParser.StatementContext context)
        {
            if(context.NEW_LINE() != null)
            {
                context.RemoveLastChild();
            }
            return base.VisitStatement(context);
        }
        #endregion
        #endregion


        #region Library functions 

        public override int VisitPrint_func([NotNull] PythonInterpreterParser.Print_funcContext context)
        {
            context.Evaluate(IntegerVariables);
            return NONE;
        }

        public override int VisitMin_func([NotNull] PythonInterpreterParser.Min_funcContext context)
        {
            var arguments = context.arguments().GetText().Split(',');
            var numbers = arguments.Select(x => GetIntValue(x));
            return numbers.Min();
        }

        public override int VisitMax_func([NotNull] PythonInterpreterParser.Max_funcContext context)
        {
            var arguments = context.arguments().GetText().Split(',');
            var numbers = arguments.Select(x => GetIntValue(x));
            return numbers.Max();
        }
        #endregion

        #region Helper methods

        private int GetIntValue(string x) => IntegerVariables.TryGetValue(x, out int value) ? value : int.Parse(x);
        
        #endregion
    }
}
