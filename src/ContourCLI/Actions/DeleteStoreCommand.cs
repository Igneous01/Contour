using CommandLine;
using ContourCore;
using ContourCore.database;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContourCLI.Actions
{
    [Verb("delete-store", HelpText = "Deletes the configuration item at the specified path")]
    class DeleteStoreCommand : AbstractCommand
    {
        [Option('p', "path", Required = true, HelpText = "Specify path via Json path expression (ie. MyConfig.Production.$MyConfigItem)")]
        public string Path { set; get; }

        public override int Execute()
        {
            try
            {
                IJsonTreeDB db = GetFactory().Create(GlobalConfig.STORE);
                db.Store.DeleteProperty(Path);
                Console.WriteLine($"Deleted {Path} from store");
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
