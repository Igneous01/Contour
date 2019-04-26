using CommandLine;
using ContourCore;
using ContourCore.database;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContourCLI.Actions
{
    [Verb("delete-profile", HelpText = "Deletes the specified configuration profile")]
    public class DeleteProfileCommand : AbstractDeleteCommand
    {
        [Option('p', "path", Required = true, HelpText = "Specify path via Json path expression (ie. MyConfig.Production.$MyConfigItem)")]
        public string Path { set; get; }

        public override int Execute()
        {
            return ExecuteDelete(GlobalConfig.PROFILE, Path, $"Deleted {Path} from profiles");
        }
    }
}
