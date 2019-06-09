using System;
using System.Collections.Generic;
using System.Text;

namespace ContourCLI
{
    public static class GlobalConfig
    {
        private static string BASE = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public static string STORE = BASE + "\\store.json";
        public static string PACKAGE = BASE + "\\package.json";
        public static string PROFILE = BASE + "\\profile.json";
    }
}
