using CommandLine;
using ContourCore;
using ContourCore.database;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContourCLI.Actions
{
    [Verb("delete-store", HelpText = "Deletes the configuration item at the specified path")]
    class DeleteStoreCommand : IShellCommand
    {
        [Option('p', "path", Required = true, HelpText = "Specify path via Json path expression (ie. MyConfig.Production.$MyConfigItem)")]
        public string Path { set; get; }

        public int Execute()
        {
            try
            {
                JsonTreeDB db = new JsonTreeDB(GlobalConfig.STORE);
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
