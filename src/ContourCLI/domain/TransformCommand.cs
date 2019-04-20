using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContourCLI.domain
{
    [Verb("transform", HelpText = "Transforms files using specified store from package, file, or wildcard")]
    class TransformCommand
    {
        [Option('p', "package", Required = false, HelpText = "Specify configuration package name")]
        public string Package { set; get; }

        [Option('f', "file", Required = false, HelpText = "Specify configuration file path")]
        public string File { set; get; }

        [Option('d', "dest", Required = true, HelpText = "Specify destination path(s) to transform")]
        public string DestinationPath { set; get; }

        //[Usage(ApplicationAlias = "ContourCLI")]
        //public static IEnumerable<Example> Examples
        //{
        //    get
        //    {
        //        return new List<Example>() {
        //            new Example("Convert file to a trendy format", new object { filename = "file.bin" })
        //        };
        //    }
        //}
    }
}
