using ContourCore.database;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContourCLI.Actions
{
    interface IShellCommand
    {
        int Execute();
        void SetFactory(IJsonTreeDBFactory factory);
    }
}
