using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiteDBPrototype
{
    public static class JsonExtensions
    {
        public static void MoveProperty(this JObject jObject, string sourcePath, string destPath)
        {
            if (sourcePath.Contains("*"))
            {
                MoveProperties(jObject, sourcePath, destPath);
            }
            else
            {
                JToken sourceToken = jObject.SelectToken(sourcePath).Parent;
                JToken destToken = jObject.SelectToken(destPath);

                if (destToken.Last != null)
                    destToken.Last.AddAfterSelf(sourceToken);
                else
                {
                    // create new JObject that holds the JProperty from t1
                    JObject newObject = new JObject { sourceToken };

                    if (destToken is JProperty destProperty)
                        destProperty.Value = newObject;
                    else
                    {
                        destProperty = destToken.Parent as JProperty;
                        destProperty.Value = newObject;
                    }
                }

                sourceToken.Remove();
            }   
        }

        private static void MoveProperties(JObject jObject, string sourcePath, string destPath)
        {
            IEnumerable<JToken> sourceTokens = jObject.SelectTokens(sourcePath).Select(token => token.Parent);
            JToken destToken = jObject.SelectToken(destPath);

            if (destToken.Last != null)
                destToken.Last.AddAfterSelf(sourceTokens);
            else
            {
                // create new JObject that holds the JProperty from t1
                JObject newObject = new JObject { sourceTokens };

                if (destToken is JProperty destProperty)
                    destProperty.Value = newObject;
                else
                {
                    destProperty = destToken.Parent as JProperty;
                    destProperty.Value = newObject;
                }
            }

            for(int i = sourceTokens.Count() - 1; i >= 0; i--)
            {
                sourceTokens.ElementAt(i).Remove();
            }
        }
    }
}
