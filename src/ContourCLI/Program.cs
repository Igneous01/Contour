using CommandLine;
using ContourCLI.domain;
using System;
using System.Linq;

namespace ContourCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<TransformCommand>(args)
                .WithParsed<TransformCommand>(action =>
                {
                    Console.WriteLine($"File: {action.File}");
                    Console.WriteLine($"DestinationPath: {action.DestinationPath}");
                    Console.WriteLine($"Package: {action.Package}");
                })
                .WithNotParsed<TransformCommand>(action =>
                {
                    foreach (Error err in action)
                        Console.WriteLine(err.ToString());

                    Console.WriteLine(action.ToString());
                });
        }
    }
}
