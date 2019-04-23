using CommandLine;
using ContourCLI.Actions;
using System;
using System.Collections.Generic;

namespace ContourCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<TransformCommand, AddStoreCommand, DeleteStoreCommand, MoveStoreCommand, GetStoreCommand, AddPackageCommand, DeletePackageCommand, GetPackageCommand, AddProfileCommand, SwitchProfileCommand>(args)
                .WithParsed<IShellCommand>(action =>
                {
                    action.Execute();
                });
            //.WithNotParsed<IShellCommand>((ParserResult<IShellCommand> result, IEnumerable<Error> Error) =>
            //{
            //    foreach (Error err in action)
            //        Console.WriteLine(err.ToString());

            //    Console.WriteLine(action.ToString());
            //});
        }
    }
}
