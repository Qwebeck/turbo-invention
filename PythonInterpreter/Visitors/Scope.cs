using System.Collections.Generic;
using System;

namespace PythonInterpreter.Visitors
{

    public class Scope
    {
        public Dictionary<string, int> Ints = new Dictionary<string, int>();
        
        public Dictionary<string, FunctionDefinition> Functions = new Dictionary<string, FunctionDefinition>();

        public Scope() { }
        public Scope(Scope scope)
            => (Ints, Functions) = (new Dictionary<string, int>(scope.Ints), new Dictionary<string, FunctionDefinition>(scope.Functions));

    }
}
