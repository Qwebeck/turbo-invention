using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PythonInterpreter.Tests.Helpers
{
    public class ConsoleMockHelper: IDisposable
    {
        private readonly StringWriter stringWriter;
        private readonly TextWriter originalTextWriter; 
        
        public ConsoleMockHelper()
        {
            stringWriter = new StringWriter();
            originalTextWriter = Console.Out;
            Console.SetOut(stringWriter);
        }

        public string Content => stringWriter.ToString();

        public void Dispose() => Console.SetOut(originalTextWriter);
        
    }
}
