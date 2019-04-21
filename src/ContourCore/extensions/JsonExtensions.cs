using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContourCore
{
    public static class JsonExtensions
    {
        public static void MoveProperty(this JObject jObject, string sourcePath, string destPath)
        {
            JToken destToken = jObject.SelectToken(destPath);

            if (destToken == null)
                throw new Exception($"Json path {destPath} not found");

            if (sourcePath.Contains("*"))
            {
                IEnumerable<JToken> sourceTokens = jObject.SelectTokens(sourcePath).Select(token => token.Parent);
                if (sourceTokens == null || sourceTokens.Count().Equals(0))
                    throw new Exception($"Json path {sourcePath} not found");

                MoveJsonProperty(destToken, sourceTokens, destPath);
                for (int i = sourceTokens.Count() - 1; i >= 0; i--)
                {
                    if (sourceTokens.ElementAt(i).Parent == null)
                        throw new Exception($"Could not find property with Json path {destPath}");

                    sourceTokens.ElementAt(i).Remove();
                }               
            }
            else
            {
                JToken sourceToken = jObject.SelectToken(sourcePath);
                if (sourceToken == null || sourceToken.Parent == null)
                    throw new Exception($"Json path {sourcePath} not found");

                MoveJsonProperty(destToken, sourceToken.Parent, destPath);
                sourceToken.Parent.Remove();
            }   
        }

        public static void DeleteProperty(this JObject jObject, string path)
        {
            IEnumerable<JToken> results = jObject.SelectTokens(path);
            if (results == null || results.Count().Equals(0))
                throw new Exception($"Json path {path} not found");

            for (int i = results.Count() - 1; i >= 0; i--)
            {
                if (results.ElementAt(i).Parent == null)
                    throw new Exception($"Could not find property with Json path {path}");

                results.ElementAt(i).Parent.Remove();
            }
                
        }

        private static void MoveJsonProperty(JToken destToken, object sourceTokens, string destPath)
        {
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
                    if (destToken.Parent == null)
                        throw new Exception($"Could not find property with Json path {destPath}");
                    destProperty = destToken.Parent as JProperty;
                    destProperty.Value = newObject;
                }
            }
        }
    }
}
