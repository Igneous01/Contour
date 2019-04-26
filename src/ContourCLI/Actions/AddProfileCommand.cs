using CommandLine;
using ContourCLI.Actions;
using ContourCore;
using ContourCore.database;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ContourCLI.Actions
{
    [Verb("add-profile", HelpText = "Add or Update profile")]
    class AddProfileCommand : AbstractCommand
    {
        [Option('p', "profile", Required = true, HelpText = "Name of profile")]
        public string Profile { set; get; }

        [Option('t', "template", Required = true, HelpText = "Template path")]
        public string TemplatePath { set; get; }

        [Option('c', "config", Separator = ';', Required = true, HelpText = "Path to configuration files - separate multiple using ;")]
        public IEnumerable<string> ConfigPaths { set; get; }

        public override int Execute()
        {
            try
            {
                ValidatePaths(ConfigPaths);
                ValidatePaths(TemplatePath);
                ConfigPaths = ConfigPaths.Select(p => System.IO.Path.GetFullPath(p)).ToList();
                JObject profileValues = new JObject
                {
                    ["Template"] = JToken.FromObject(TemplatePath),
                    ["Config"] = JToken.FromObject(ConfigPaths)
                };

                IJsonTreeDB profileDB = GetFactory().Create(GlobalConfig.PROFILE);
                profileDB.Store.CreateProperty(Profile, profileValues);
                Console.WriteLine($"Added Profile {Profile}");
                profileDB.Write();
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
            foreach (string p in paths)
            {
                if (!File.Exists(p))
                    throw new FileNotFoundException($"Could not find file with the path {System.IO.Path.GetFullPath(p)}");
            }
        }

        private void ValidatePaths(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"Could not find file with the path {System.IO.Path.GetFullPath(path)}");
        }
    }
}
