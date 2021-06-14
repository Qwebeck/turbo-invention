using System.Collections.Generic;
using PythonInterpreter.Grammar;


namespace PythonInterpreter
{
    public static class IFStatementContextExtensions
    {
        public static List<ValidationError> Validate(this PythonInterpreterParser.If_statementContext context)
        {
            var validationErrors = new List<ValidationError>();
            if (context.END() == null)
            {
                validationErrors.Add(new ValidationError
                {
                    Statement = "IF",
                    Message = "END token is missing",
                    StatementInterval = context.SourceInterval
                });
            }
            return validationErrors;
        }
    }
}
