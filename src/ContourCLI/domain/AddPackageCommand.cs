﻿using CommandLine;
using CommandLine.Text;
using ContourCore;
using ContourCore.database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContourCLI.domain
{
    [Verb("add-package", HelpText = "Add or Update configuration package")]
    class AddPackageCommand : IShellCommand
    {
        [Option('p', "path", Required = true, HelpText = "Name of configuration package")]
        public string Path { set; get; }

        [Option('v', "value", Separator = ';', Required = true, HelpText = "Configuration store path values - separate multiple via ;")]
        public IEnumerable<string> Value { set; get; }

        public int Execute()
        {
            try
            {
                ValidatePaths(Value);

                JsonTreeDB packageDB = new JsonTreeDB(GlobalConfig.PACKAGE);
                packageDB.Store.CreateProperty(Path, Value);
                Console.WriteLine($"Added Package {Path} with value {Value}");
                packageDB.Write();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return -1;
            }

            Console.WriteLine("Completed Successfully");
            return 0;
        }

        private void ValidatePaths(IEnumerable<string> paths)
        {
            JsonTreeDB storeDB = new JsonTreeDB(GlobalConfig.STORE);
            foreach(string p in paths)
            {
                if (storeDB.Store.SelectTokens(p).ToList().Count == 0)
                    throw new Exception($"Error: the configuration store path {p} does not exist");
            }
        }
    }
}