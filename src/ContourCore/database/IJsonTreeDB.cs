using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContourCore.database
{
    public interface IJsonTreeDB
    {
        JObject Store { get; }
        void Write();
        void Read();
    }
}
