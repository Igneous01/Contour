using CommandLine;
using ContourCore;
using ContourCore.database;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContourCLI.Actions
{
    [Verb("get-package", HelpText = "Get defined package")]
    class GetPackageCommand : AbstractCommand
    {
        [Option('p', "path", Required = true, HelpText = "Name of configuration package")]
        public string Path { set; get; }

        public override int Execute()
        {
            try
            {
                IJsonTreeDB db = GetFactory().Create(GlobalConfig.PACKAGE);             
                Console.WriteLine($"Results for {Path} :");
                JToken results = db.Store.SelectToken(Path).Parent;
                if (results != null)
                    Console.WriteLine($"{results.Path}: {(results as JProperty)?.Value}");
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
