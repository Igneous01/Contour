using CommandLine;
using CommandLine.Text;
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
    [Verb("transform", HelpText = "Transforms files using specified store from package, file, or wildcard")]
    class TransformCommand : AbstractCommand
    {
        [Option('p', "package", Required = false, HelpText = "Specify configuration package name")]
        public string PackageName { set; get; }

        [Option('o', "object", Required = false, HelpText = "Specify configuration object path")]
        public string ObjectPath { set; get; }

        [Option('d', "dest", Separator = ';', Required = true, HelpText = "Specify destination path(s) to transform - separate using ;")]
        public IEnumerable<string> DestinationPath { set; get; }

        [Option('t', "template", Required = true, HelpText = "Specify template file to use in transform")]
        public string TemplatePath { set; get; }

        public override int Execute()
        {
            if (ObjectPath == null && PackageName == null)
            {
                Console.WriteLine("Missing required argument --package or --object");
                return -1;
            }

            try
            {
                IDictionary<string, object> results;
                string errorNotFound;

                if (PackageName != null)
                {
                    IJsonTreeDB packageDB = GetFactory().Create(GlobalConfig.PACKAGE);
                    IEnumerable <string> paths = (packageDB.Store.SelectToken(PackageName).Parent as JProperty).Value.ToObject<IEnumerable<string>>();
                    results = new Dictionary<string, object>();
                    IJsonTreeDB configDB = GetFactory().Create(GlobalConfig.STORE);

                    foreach (string p in paths)
                    {
                        IDictionary<string, object> r = configDB.Store
                                                .FindAllProperties(p)
                                                .Cast<JProperty>()
                                                .ToDictionary(kvp => kvp.Name, kvp => kvp.Value.ToObject(typeof(Object)));
                        results = results.Concat(r)
                                    .ToLookup(x => x.Key, x => x.Value)
                                    .ToDictionary(x => x.Key, g => g.First());
                    }

                    errorNotFound = $"Error in transform: Package {PackageName} not found";
                }
                else
                {
                    JsonTreeDB configDB = new JsonTreeDB(GlobalConfig.STORE);
                    results = configDB.Store
                                        .FindAllProperties(ObjectPath)
                                        .Cast<JProperty>()
                                        .ToDictionary(kvp => kvp.Name, kvp => kvp.Value.ToObject(typeof(Object)));

                    errorNotFound = $"Error in transform: Config {ObjectPath} not found";
                }

                if (results == null || results.Count == 0)
                {
                    Console.WriteLine(errorNotFound);
                    return -1;
                }

                Console.WriteLine($"Reading file {TemplatePath}");
                string content = File.ReadAllText(TemplatePath);
                string transformedContent = Transformer.Transform(content, results);

                foreach (string p in DestinationPath)
                {
                    Console.WriteLine($"Transforming file {p}");
                    Console.WriteLine($"Saving file {p}");
                    File.WriteAllText(p, transformedContent);
                }             
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in transform: " + ex.Message);
                return -1;
            }

            Console.WriteLine("Completed Successfully");
            return 0;
        }

        [Usage(ApplicationAlias = "ContourCLI")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                return new List<Example>() {
                    new Example("Transform set of files", "transform -o MyConfig.Path.* -d Some\\Config\\Path\\my.config -t Some\\Config\\mytemplate.config"),
                    new Example("Transform set of files", "transform --object MyConfig.Path.* --dest Some\\Config\\Path\\my.config --template Some\\Config\\mytemplate.config")
                };
            }
        }
    }
}
