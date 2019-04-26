using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContourCore.database
{
    public class MockJsonTreeDB : IJsonTreeDB
    {
        public static Dictionary<string, JObject> _Store = new Dictionary<string, JObject>();

        private string _Path;

        public JObject Store { get => _Store[_Path]; private set => _Store[_Path] = value; }

        public MockJsonTreeDB(string path)
        {
            _Path = path;
            Read();
        }

        public void Write()
        {
        }

        public void Read()
        {
            if (!_Store.ContainsKey(_Path))
                _Store[_Path] = new JObject();
        }

        public static void Clear()
        {
            _Store = new Dictionary<string, JObject>();
        }
    }
}
