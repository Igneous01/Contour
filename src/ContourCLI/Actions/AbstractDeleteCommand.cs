using ContourCore;
using ContourCore.database;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContourCLI.Actions
{
    public abstract class AbstractDeleteCommand : AbstractCommand, IShellCommand
    {
        protected int ExecuteDelete(string dbPath, string jsonPath, string messageContext)
        {
            try
            {
                IJsonTreeDB db = GetFactory().Create(dbPath);
                db.Store.DeleteProperty(jsonPath);
                Console.WriteLine(messageContext);
                db.Write();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return -1;
            }

            Console.WriteLine("Completed Successfully");
            return 0;
        }
    }
}
