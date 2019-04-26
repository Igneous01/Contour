using CommandLine;
using ContourCore;
using ContourCore.database;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContourCLI.Actions
{
    [Verb("add-store", HelpText = "Add or updates a configuration item at the specified path with the specified value")]
    public class AddStoreCommand : AbstractCommand
    {
        [Option('p', "path", Required = true, HelpText = "Specify path via Json path expression (ie. MyConfig.Production.$MyConfigItem)")]
        public string Path { set; get; }

        [Option('v', "value", Required = true, HelpText = "Specify value")]
        public string Value { set; get; }

        public override int Execute()
        {
            try
            {
                IJsonTreeDB db = GetFactory().Create(GlobalConfig.STORE);
                db.Store.CreateProperty(Path, Value);
                Console.WriteLine($"Added {Path} to store with value {Value}");
                db.Write();
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
