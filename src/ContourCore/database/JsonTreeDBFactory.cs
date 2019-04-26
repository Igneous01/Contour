using System;
using System.Collections.Generic;
using System.Text;

namespace ContourCore.database
{
    public class JsonTreeDBFactory : IJsonTreeDBFactory
    {
        public IJsonTreeDB Create(string path)
        {
            return new JsonTreeDB(path);
        }
    }
}
