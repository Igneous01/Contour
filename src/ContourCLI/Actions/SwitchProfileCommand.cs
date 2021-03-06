﻿using CommandLine;
using ContourCore.database;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContourCLI.Actions
{
    [Verb("switch-profile", HelpText = "Add or Update profile")]
    class SwitchProfileCommand : AbstractCommand
    {
        [Option('n', "name", Required = true, HelpText = "Name of profile")]
        public string Profile { set; get; }

        [Option('p', "package", Required = true, HelpText = "Name of package")]
        public string Package { set; get; }

        public override int Execute()
        {
            try
            {
                IJsonTreeDB profileDB = GetFactory().Create(GlobalConfig.PROFILE);
                JObject profile = profileDB.Store.SelectToken(Profile) as JObject;
                if (profile == null)
                {
                    Console.WriteLine($"Profile '{Profile}' not found.");
                    return -1;
                }
                TransformCommand cmd = new TransformCommand()
                {
                    PackageName = Package,
                    DestinationPath = profile["Config"].ToObject<IEnumerable<string>>(),
                    TemplatePath = profile["Template"].ToObject<string>()
                };

                return cmd.Execute();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return -1;
            }
        }
    }
}
