using System;
using System.Collections.Generic;
using System.Text;
using ContourCore.database;

namespace ContourCLI.Actions
{
    public abstract class AbstractCommand : IShellCommand
    {
        private IJsonTreeDBFactory Factory;

        public abstract int Execute();

        public void SetFactory(IJsonTreeDBFactory factory)
        {
            Factory = factory;
        }

        protected IJsonTreeDBFactory GetFactory()
        {
            if (Factory != null)
                return Factory;
            else
            {
                Factory = new JsonTreeDBFactory();
                return Factory;
            }
        }
    }
}
