﻿using CommandLine;
using ContourCore;
using ContourCore.database;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContourCLI.Actions
{
    [Verb("delete-store", HelpText = "Deletes the configuration item at the specified path")]
    public class DeleteStoreCommand : AbstractDeleteCommand
    {
        [Option('p', "path", Required = true, HelpText = "Specify path via Json path expression (ie. MyConfig.Production.$MyConfigItem)")]
        public string Path { set; get; }

        public override int Execute()
        {
            return ExecuteDelete(GlobalConfig.STORE, Path, $"Deleted {Path} from store");
        }
    }
}
