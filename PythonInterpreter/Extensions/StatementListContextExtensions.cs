using System.Collections.Generic;
using PythonInterpreter.Grammar;


namespace PythonInterpreter
{
    public static class StatementListContextExtensions
    {
        public static int Evaluate(this PythonInterpreterParser.Statement_listContext context, Dictionary<string, int> variables)
        {
            context.statement()[0].function_call_statement().library_func().print_func().Evaluate(variables);
            return 0;
        }
    }


}
