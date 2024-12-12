using System;
using System.Threading.Tasks;
using Astator.Script;
using Jint;

namespace _02.引用nuget包
{
    public class Program
    {
        [EntryMethod]
        public static async Task MainAsync(string workDir)
        {
            var engine = new Engine()
                .SetValue("log", new Action<object>(Console.WriteLine));

            engine.Execute(@"
    function hello() { 
        log('Hello World form nuget package: Jint');
    };
 
    hello();
");
        }
    }

}
