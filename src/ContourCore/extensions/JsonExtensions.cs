using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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

        public static void CreateProperty(this JObject jObject, string path, object value)
        {
            Regex regex = new Regex(@"[\[\]\\\/*]");
            if (regex.Match(path).Success)
                throw new ArgumentException($"Illegal path supplied - {path} is not valid");

            string[] properties = path.Split(".");

            if (properties.Any(s => String.IsNullOrWhiteSpace(s)))
                throw new ArgumentException($"Illegal path supplied - {path} is not valid");

            JToken innerJObject = jObject;

            foreach (string p in properties)
                innerJObject = GetOrCreateProperty(innerJObject, p);

            //if (!(innerJObject is JValue))
                (innerJObject.Parent as JProperty).Value = JToken.FromObject(value);
            //else
            //    (innerJObject as JProperty).Value = JToken.FromObject(value);
        }

        public static IEnumerable<JToken> FindAllProperties(this JObject jObject)
        {
            return jObject.Descendants()
                        .OfType<JProperty>()
                        .Where(p => IsJPropertyValidType(p))
                        .Select(p => p);
        }

        public static IEnumerable<JToken> FindAllProperties(this JObject jObject, string path)
        {
            var result1 = jObject.SelectTokens(path)
                        .Where(t => t is JObject)
                        .Cast<JObject>()
                        .Descendants()
                        .OfType<JProperty>()
                        .Where(p => IsJPropertyValidType(p))
                        .Select(p => p);

            var result2 = jObject.SelectTokens(path)
                        .Select(p => p.Parent)
                        .OfType<JProperty>()
                        .Where(p => IsJPropertyValidType(p))
                        .Select(p => p);

            return result1.Concat(result2);
        }

        private static bool IsJPropertyValidType(JProperty jProperty)
        {
            IEnumerable<JTokenType> ValidTypes = new List<JTokenType>()
                {
                    JTokenType.String,
                    JTokenType.Integer,
                    JTokenType.Boolean,
                    JTokenType.Bytes,
                    JTokenType.Date,
                    JTokenType.Float,
                    JTokenType.Guid,
                    JTokenType.TimeSpan,
                    JTokenType.Uri
                };

            return ValidTypes.Any(jt => jt == jProperty.Value.Type);
        }

        private static JToken GetOrCreateProperty(JToken jObject, string property)
        {
            if (jObject is JValue)
            {
                JObject newValue = new JObject { { property, new JObject() } };
                JProperty jProperty = jObject.Parent as JProperty;
                jProperty.Value = JToken.FromObject(newValue);
                return jProperty.Value[property];
            }
            else
            {
                JToken jt = jObject[property];

                if (jt != null)
                    return jt;
                else
                {
                    JProperty jProperty = jObject as JProperty;
                    if (jProperty == null)
                    {
                        jObject[property] = new JObject();
                        return jObject[property];
                    }
                    else
                    {
                        JObject newValue = new JObject { { property, new JObject() } };
                        jProperty.Value = JToken.FromObject(newValue);
                        return jProperty.Value[property];
                    }
                   
                }
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
