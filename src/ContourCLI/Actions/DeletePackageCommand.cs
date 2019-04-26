using CommandLine;
using ContourCore;
using ContourCore.database;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContourCLI.Actions
{
    [Verb("delete-package", HelpText = "Deletes the specified package")]
    class DeletePackageCommand : AbstractCommand
    {
        [Option('p', "path", Required = true, HelpText = "Specify path via Json path expression (ie. MyConfig.Production.$MyConfigItem)")]
        public string Path { set; get; }

        public override int Execute()
        {
            try
            {
                IJsonTreeDB db = GetFactory().Create(GlobalConfig.PACKAGE);
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
