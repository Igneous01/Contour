using CommandLine;
using ContourCore;
using ContourCore.database;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContourCLI.Actions
{
    [Verb("delete-package", HelpText = "Deletes the specified package")]
    public class DeletePackageCommand : AbstractDeleteCommand
    {
        [Option('p', "path", Required = true, HelpText = "Specify path via Json path expression (ie. MyConfig.Production.$MyConfigItem)")]
        public string Path { set; get; }

        public override int Execute()
        {
            return ExecuteDelete(GlobalConfig.PACKAGE, Path, $"Deleted {Path} from package store");
        }
    }
}
