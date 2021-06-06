using Antlr4.Runtime.Misc;
using PythonInterpreter.Grammar;

namespace PythonInterpreter
{
    class AdditionParsing : PythonInterpreterBaseVisitor<int>
    {
        public override int VisitEquation([NotNull] PythonInterpreterParser.EquationContext context)
        {
            int left = int.Parse(context.INT()[0].GetText());
            int right = int.Parse(context.INT()[1].GetText());
            return left + right;
        }
    }
}
