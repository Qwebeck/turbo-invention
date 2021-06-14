using System.Collections.Generic;
using System.Linq;
using PythonInterpreter.Grammar;


namespace PythonInterpreter
{
    class FunctionDefinition
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
            

     
        public Dictionary<string, int> CreateLocalScope(string stringSeparatedVariables, Dictionary<string, int> globalVariables)
        {
            var valuesToAssign = stringSeparatedVariables.Split(',').Select(var => globalVariables.TryGetValue(var, out int val) ? val  : int.Parse(var));
            var localScope = new Dictionary<string, int>(Parameters);
            localScope.Keys.Zip(valuesToAssign).ForEach(item =>
            {
                localScope[item.First] = item.Second;
            });
            return localScope;
        }
    }
}
