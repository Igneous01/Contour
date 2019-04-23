using CommandLine;
using ContourCore;
using ContourCore.database;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContourCLI.Actions
{
    [Verb("get-store", HelpText = "Get configuration value from store")]
    class GetStoreCommand : IShellCommand
    {
        [Option('p', "path", Required = true, HelpText = "Configuration store path")]
        public string Path { set; get; }

        public int Execute()
        {
            try
            {
                JsonTreeDB db = new JsonTreeDB(GlobalConfig.STORE);
                IEnumerable<JToken> results = db.Store.FindAllProperties(Path);
                Console.WriteLine($"Results for {Path} :");
                if (results.ToList().Count == 0)
                    Console.WriteLine("No results");
                foreach(JToken t in results)
                    Console.WriteLine($"{t.Path}: {(t as JProperty)?.Value}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return -1;
            }
            Console.WriteLine("Completed Successfully");
            return 0;
        }
    }
}
