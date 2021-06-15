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
        private Scope GlobalScope = new Scope();
        #endregion

        #region function handling
 
        public override int VisitFunction_definition([NotNull] PythonInterpreterParser.Function_definitionContext context)
        {
            var signature = context.function_signature();
            var functionName = signature.ID().ToString();

            var functionParams = signature.parameters().ID().Select(id => id.GetText());
            // TODO fix to handle more sophisticated variables
            var functionDefinition = new FunctionDefinition(functionName, functionParams, context.function_body());
            GlobalScope.Functions[functionName] = functionDefinition;
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
            var globalScope = GlobalScope;
            var calledFunction = GlobalScope.Functions[functionName];
            GlobalScope = calledFunction.CreateLocalScope(arguments, globalScope);
            var result = base.VisitFunction_body(calledFunction.FunctionBody);
            GlobalScope = globalScope;
            return result;
        }

        public override int VisitReturnStatement([NotNull] PythonInterpreterParser.ReturnStatementContext context)
        {
            var statement = context.statement();
            var result = new MainVisitor 
            { 
                GlobalScope = new Scope(GlobalScope)
            }.VisitStatement(statement);
            return result;
                
        }
        public override int VisitEmptyFunctionEnd([NotNull] PythonInterpreterParser.EmptyFunctionEndContext context)
        {
            return NONE;
        }
        #endregion

        #region Statements 

        public override int VisitAssignment_statement([NotNull] PythonInterpreterParser.Assignment_statementContext context)
        {
            var name = context.ID().GetText();
            var value = base.Visit(context.expression());
            GlobalScope.Ints[name] = value;
            return NONE;
        }

        public override int VisitIf_statement([NotNull] PythonInterpreterParser.If_statementContext context)
        {
            var isConditionTrue = new ConditionVisitor(GlobalScope).Visit(context.condition()[0]);
            var stamentLists = context.statement_list();
            if (isConditionTrue)
            {
                var statement = context.statement_list()[0];
                return base.Visit(statement);
            }
            
            if (context.ELSEIF().Length > 0)
            {
                var matchedStatement = context.condition().Zip(stamentLists).Skip(1).FirstOrDefault(pair => new ConditionVisitor(GlobalScope).Visit(pair.First));
                if (!matchedStatement.Equals(default))
                {
                    return base.Visit(matchedStatement.Second);
                }
    
            }
            
            if (context.ELSE() != null)
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

        public override int VisitMathFactor([NotNull] PythonInterpreterParser.MathFactorContext context)
        {
            return (context.factor(), context.variable()) switch
            {
                (var value, null) => base.Visit(value),
                (null, var variable) => base.Visit(variable),
                (_, _) => throw new ArgumentException($"Bad context passed in {nameof(VisitFactor)}")
            };  
        }

        public override int VisitFactor([NotNull] PythonInterpreterParser.FactorContext context)
        {
            var value = context.INT().GetText();
            return int.Parse(value);
        }
        public override int VisitVariable([NotNull] PythonInterpreterParser.VariableContext context)
        {
            var variable = context.ID().GetText();
            return GlobalScope.Ints.TryGetValue(variable, out int result) 
                    ? result 
                    : throw new ArgumentException($"Variable {variable} not exists in scope");
        }

        public override int VisitParenthesedStatemet([NotNull] PythonInterpreterParser.ParenthesedStatemetContext context)
        {
            var statement = context.math_statement();
            return base.Visit(statement);
        }
        public override int VisitStatement([NotNull] PythonInterpreterParser.StatementContext context)
        {
            while(context.NEW_LINE().LastOrDefault()!= null) // escaping new line, so program will return last executed value
            {
                context.RemoveLastChild();
            }
             return base.VisitStatement(context);
        }

        public override int VisitProgram([NotNull] PythonInterpreterParser.ProgramContext context)
        {
            if(context.Eof() != null) // escaping eof, so program will return last executed value
            {
                context.RemoveLastChild();
            }
            return base.VisitProgram(context);
        }

        #endregion
        #endregion


        #region Library functions 

        public override int VisitPrint_func([NotNull] PythonInterpreterParser.Print_funcContext context)
        {
            var arguments = context.arguments().argument();
            var values = arguments.Select(arg => arg.@string() == null 
                                                ? base.Visit(arg).ToString()
                                                : arg.GetText()
                                         );
            var result = string.Join(' ', values).Trim(' ');
            Console.WriteLine(result);
            return NONE;
        }


        public override int VisitMin_func([NotNull] PythonInterpreterParser.Min_funcContext context)
        {
            var arguments = context.arguments().GetText().Split(',');
            var numbers = arguments.Select(x =>GetIntValue(x));
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

        private int GetIntValue(string x) => GlobalScope.Ints.TryGetValue(x, out int value) ? value : int.Parse(x);
        
        #endregion
    }
}
