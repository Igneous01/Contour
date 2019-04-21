using ContourCore;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnit.ContourCore
{
    [TestFixture]
    public class TransformerTests
    {
        public static IEnumerable<TestCaseSource> Source
        {
            get
            {
                return new List<TestCaseSource>()
                {
                    new TestCaseSource()
                    {
                        Content = "This is a ${A} string with ${B} and ${A}",
                        ReplacedContent = "This is a Valid string with Keys and Valid",
                        Properties = new Dictionary<string, object>()
                        {
                            { "A", "Valid" },
                            { "B", "Keys" }
                        }
                    },
                    new TestCaseSource()
                    {
                        Content = "This is a ${A} string with ${B} and ${A}",
                        ReplacedContent = "This is a Replaced string with ${B} and Replaced",
                        Properties = new Dictionary<string, object>()
                        {
                            { "A", "Replaced" },
                        }
                    },
                    new TestCaseSource()
                    {
                        Content = "This is a ${A} with ${B} and ${C}",
                        ReplacedContent = "This is a Dog with Cat and ${C}",
                        Properties = new Dictionary<string, object>()
                        {
                            { "A", "Dog" },
                            { "B", "Cat" },
                            { "D", "Tree" }
                        }
                    },
                    new TestCaseSource()
                    {
                        Content = "This is a ${A} with ${B} and ${C}",
                        ReplacedContent = "This is a Dog with Cat and Tree",
                        Properties = new Dictionary<string, object>()
                        {
                            { "A", "Dog" },
                            { "B", "Cat" },
                            { "C", "Tree" }
                        }
                    }
                };
            }
        }

        [TestCaseSource(nameof(Source))]
        public void TransformWorks(TestCaseSource source)
        {
            string result = Transformer.Transform(source.Content, source.Properties);
            Assert.That(result, Is.EqualTo(source.ReplacedContent), $"Transformed content {result} does not match expected {source.ReplacedContent}");
        }

        public class TestCaseSource
        {
            public string Content { get; set; }
            public string ReplacedContent { get; set; }
            public IDictionary<string, object> Properties { get; set; }
        }
    }
}
