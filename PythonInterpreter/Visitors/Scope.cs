using System.Collections.Generic;


namespace PythonInterpreter.Visitors
{
    public class Scope
    {
        public Dictionary<string, int> IntegerVariables = new Dictionary<string, int>();

        public Dictionary<string, FunctionDefinition> Functions = new Dictionary<string, FunctionDefinition>();


        public Scope() { }
        public Scope(Scope scope)
            => (IntegerVariables, Functions) = (new Dictionary<string, int>(scope.IntegerVariables), new Dictionary<string, FunctionDefinition>(scope.Functions));
    }
}
