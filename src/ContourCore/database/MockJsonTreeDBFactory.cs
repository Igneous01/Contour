using System;
using System.Collections.Generic;
using System.Text;

namespace ContourCore.database
{
    public class MockJsonTreeDBFactory : IJsonTreeDBFactory
    {
        public IJsonTreeDB Create(string path)
        {
            return new MockJsonTreeDB(path);
        }
    }
}
