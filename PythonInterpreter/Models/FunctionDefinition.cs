using System.Collections.Generic;
using System.Linq;
using PythonInterpreter.Grammar;
using PythonInterpreter.Visitors;

namespace PythonInterpreter
{
    public class FunctionDefinition
    {
        public readonly string FunctionName;
        public readonly Dictionary<string, int> Parameters;
        public readonly PythonInterpreterParser.Function_bodyContext FunctionBody;

        public FunctionDefinition(string functionName, IEnumerable<string> parameters, PythonInterpreterParser.Function_bodyContext functionBody)
        {
            FunctionName = functionName;
            Parameters = parameters.Select(s => s.Trim(' ')).ToDictionary(paramName => paramName, paramName => 0);// TODO: handle null values and 
            // other strange beasts
            FunctionBody = functionBody;
        }
            

     
        public Scope CreateLocalScope(string stringSeparatedVariables, Scope globalScope)
        {
            var valuesToAssign = stringSeparatedVariables.Split(',').Select(var => globalScope.Ints.TryGetValue(var, out int val) ? val  : int.Parse(var));
            var localScope = new Scope(globalScope);
            Parameters.Keys.Zip(valuesToAssign).ForEach(item =>
            {
                localScope.Ints[item.First] = item.Second;
            });
            return localScope;
        }
    }
}
