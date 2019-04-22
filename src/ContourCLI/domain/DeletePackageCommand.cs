using CommandLine;
using ContourCore;
using ContourCore.database;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContourCLI.domain
{
    [Verb("delete-package", HelpText = "Deletes the specified package")]
    class DeletePackageCommand : IShellCommand
    {
        [Option('p', "path", Required = true, HelpText = "Specify path via Json path expression (ie. MyConfig.Production.$MyConfigItem)")]
        public string Path { set; get; }

        public int Execute()
        {
            try
            {
                JsonTreeDB db = new JsonTreeDB(GlobalConfig.PACKAGE);
                db.Store.DeleteProperty(Path);
                Console.WriteLine($"Deleted {Path} from package store");
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
