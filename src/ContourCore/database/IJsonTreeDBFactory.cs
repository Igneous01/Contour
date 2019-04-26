using System;
using System.Collections.Generic;
using System.Text;

namespace ContourCore.database
{
    public interface IJsonTreeDBFactory
    {
        IJsonTreeDB Create(string path);
    }
}
