namespace PythonInterpreter
{
    public struct ValidationError
    {
        public string Statement;
        public string Message;
        public Antlr4.Runtime.Misc.Interval StatementInterval;
        public override string ToString()
        {
            return $"Error in {Statement} statement: {Message} at positions {StatementInterval.a}-{StatementInterval.b}";
        }
    }
}
