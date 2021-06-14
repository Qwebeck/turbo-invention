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
        public static int ERROR_EXIT_CODE => 1;
        public static int SUCCESS_EXIT_CODE => 0;

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
            // TODO: handle void and return 
            base.VisitFunction_body(calledFunction.FunctionBody);
            IntegerVariables = globalScope;
            return SUCCESS_EXIT_CODE;
        }
        #endregion

        #region Statements 
        public override int VisitAssignment_statement([NotNull] PythonInterpreterParser.Assignment_statementContext context)
        {
            var name = context.ID().GetText();
            var value = int.Parse(context.expression().INT().GetText());
            IntegerVariables[name] = value;
            return base.VisitAssignment_statement(context);
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
            return SUCCESS_EXIT_CODE;
        }
        #endregion


        #region Library functions 

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
        #endregion



        #region Helper methods

        private int GetIntValue(string x) => IntegerVariables.TryGetValue(x, out int value) ? value : int.Parse(x);
        
        #endregion
    }
}
