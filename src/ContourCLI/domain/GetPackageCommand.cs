using CommandLine;
using ContourCore;
using ContourCore.database;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContourCLI.domain
{
    [Verb("get-package", HelpText = "Get defined package")]
    class GetPackageCommand : IShellCommand
    {
        [Option('p', "path", Required = true, HelpText = "Name of configuration package")]
        public string Path { set; get; }

        public int Execute()
        {
            try
            {
                JsonTreeDB db = new JsonTreeDB(GlobalConfig.PACKAGE);             
                Console.WriteLine($"Results for {Path} :");
                JToken results = db.Store.SelectToken(Path);
                if (results != null)
                    Console.WriteLine($"{results.Path}: {results}");
                else
                    Console.WriteLine("No Results");
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
