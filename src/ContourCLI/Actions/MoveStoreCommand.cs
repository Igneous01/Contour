using CommandLine;
using ContourCore;
using ContourCore.database;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContourCLI.Actions
{
    [Verb("move-store", HelpText = "Move a configuration store to another location")]
    class MoveStoreCommand : AbstractCommand
    {
        [Option('p', "path", Required = true, HelpText = "Specify source path via Json path expression (ie. MyConfig.Production.$MyConfigItem)")]
        public string SourcePath { set; get; }

        [Option('d', "dest", Required = true, HelpText = "Specify dest path via Json path expression")]
        public string DestPath { set; get; }

        public override int Execute()
        {
            try
            {
                IJsonTreeDB db = GetFactory().Create(GlobalConfig.STORE);
                db.Store.MoveProperty(SourcePath, DestPath);
                Console.WriteLine($"Moved {SourcePath} to {DestPath}");
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
