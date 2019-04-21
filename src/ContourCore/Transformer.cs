using System;
using System.Collections.Generic;
using System.Text;

namespace ContourCore
{
    public static class Transformer
    {
        public static string Transform(string contentToTransform, IDictionary<string, object> properties)
        {
            string result = contentToTransform;
            foreach(KeyValuePair<string, object> property in properties)
            {
                string decoratedProperty = "${" + property.Key + "}";
                result = result.Replace(decoratedProperty, property.Value.ToString());
            }

            return result;
        }
    }
}
