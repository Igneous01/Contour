using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContourCore.database
{
    public class MockJsonTreeDB : IJsonTreeDB
    {
        public static JObject _Store;

        public JObject Store { get => _Store; private set => _Store = value; }

        public MockJsonTreeDB()
        {
            Read();
        }

        public void Write()
        {
        }

        public void Read()
        {
            if (Store == null)
                Store = new JObject();
        }

        public static void Clear()
        {
            _Store = new JObject();
        }
    }
}
