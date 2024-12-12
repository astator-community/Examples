using System;
using System.Threading.Tasks;
using Astator.Script;

namespace HelloWorld
{
    public class Program
    {
        [EntryMethod]
        public static async Task MainAsync(string workDir)
        {
            Console.WriteLine("Hello world form astator");
        }
    }

}
