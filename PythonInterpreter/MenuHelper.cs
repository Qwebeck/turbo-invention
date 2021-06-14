using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace PythonInterpreter
{
    class MenuHelper
    {
        public static bool InputType()
        {
            Console.Write("If you want to provide a prepared file write Y in console: ");
            string path = Console.ReadLine();
            if (path == "Y")
                return true;
            return false;                   
        }
        public static string GetFile()
        {
            try
            {
                Console.Write("Provide path to file: ");
                string path = Console.ReadLine();
                string text = GetFileContents(path);
                Console.WriteLine($"Contents of {path} = {0}", text);
                return text;
            }
            catch(Exception ex)
            {
                Console.Write("File does not exist. Press any key to restart the program.");
                Console.ReadLine();
                return "Nothing to Parse";
            }
        }

        public static string GetFileContents(string path)
        {
            try 
            {
                return File.ReadAllText(path);
            } 
            catch(FileNotFoundException)
            {
                throw new ArgumentException($"File at {path} does not exists");
            }
            
        }
    }
}
