using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AIAutomation.Resources
{
    public class AIYamlTestGenerator
    {
        private const string API_KEY = "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";
        private const string API_URL = "https://openrouter.ai/api/v1/chat/completions";

        public static async Task Main(string[] args)
        {
            var testCaseFile = args.Length > 0 ? args[0] : throw new ArgumentException("No YAML file provided.");
            var yamlPath = Path.Combine("AIAutomation.TestCases.TestSteps", testCaseFile);
            var yamlText = File.ReadAllText(yamlPath);

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(UnderscoredNamingConvention.Instance)
                .IgnoreUnmatchedProperties()
                .Build();

            var testCase = deserializer.Deserialize<YamlTestCase>(yamlText);

            var builder = new StringBuilder();
            builder.AppendLine("using NUnit.Framework;");
            builder.AppendLine("using OpenQA.Selenium;");
            builder.AppendLine("using OpenQA.Selenium.Chrome;");
            builder.AppendLine("using System.Threading;");
            builder.AppendLine("");
            builder.AppendLine($"namespace GeneratedTests {{");
            builder.AppendLine($"    [TestFixture]");
            builder.AppendLine($"    public class {testCase.TestName} {{");
            builder.AppendLine($"        IWebDriver driver;");
            builder.AppendLine("        [SetUp]");
            builder.AppendLine("        public void Setup() => driver = new ChromeDriver();");
            builder.AppendLine("        [TearDown]");
            builder.AppendLine("        public void Teardown() => driver.Quit();");
            builder.AppendLine($"        [Test]");
            builder.AppendLine($"        public void {testCase.TestName}Steps() {{");

            foreach (var step in testCase.TestSteps)
            {
                if (!string.IsNullOrEmpty(step.Method))
                {
                    builder.AppendLine($"            {step.Method}({BuildParams(step.Params)});");
                }
                else
                {
                    var aiCode = await GetCodeFromAI(testCase, step);
                    builder.AppendLine(aiCode);
                }
            }

            builder.AppendLine("        }");
            builder.AppendLine("    }");
            builder.AppendLine("}");

            var outputDir = "AIAutomation.TestCases.AutomatedTestSteps";
            Directory.CreateDirectory(outputDir);
            var outputPath = Path.Combine(outputDir, $"{testCase.TestName}.cs");
            File.WriteAllText(outputPath, builder.ToString());
            Console.WriteLine($"✅ Generated {outputPath}");
        }

        private static string BuildParams(Dictionary<string, string> paramDict)
        {
            if (paramDict == null) return "";
            var paramList = new List<string>();
            foreach (var pair in paramDict)
                paramList.Add($"\"{pair.Value}\"");
            return string.Join(", ", paramList);
        }

        private static async Task<string> GetCodeFromAI(YamlTestCase testCase, TestStep step)
        {
            string prompt = @$"
                                You are an expert C# Selenium test automation engineer.

                                Generate a complete C# NUnit Selenium WebDriver code snippet to perform the action '{step.Action}' using the provided parameters:
                                {JsonConvert.SerializeObject(step.Params)}

                                {(string.IsNullOrWhiteSpace(step.Instructions) ? "" : $"Additional instructions: {step.Instructions}")}

                                Use standard C# helper methods if suitable.

                                Do not include driver setup, teardown, or class/test declarations.

                                Only provide the code snippet for this step. No explanations.

                            ";

            var requestBody = new
            {
                model = "mistralai/devstral-small:free",
                messages = new[]
                {
                new { role = "system", content = "You are a C# Selenium test assistant. Only return code in response." },
                new { role = "user", content = prompt }
            }
            };

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", API_KEY);
            client.DefaultRequestHeaders.Add("X-Title", "SeleniumYamlGenerator");

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(API_URL, content);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"❌ API call failed: {response.StatusCode}");
                return $"// Failed to get AI code for action: {step.Action}";
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<AIResponse>(json);
            return "            " + result.choices[0].message.content.Trim().Replace("\n", "\n            ");
        }

        // YAML mapping classes
        public class YamlTestCase
        {
            public string TestName { get; set; }
            public List<TestStep> TestSteps { get; set; }
        }

        public class TestStep
        {
            public string Action { get; set; }
            public string Method { get; set; }
            public Dictionary<string, string> Params { get; set; }
            public string Instructions { get; set; }
        }

        public class AIResponse
        {
            public List<Choice> choices { get; set; }

            public class Choice
            {
                public Message message { get; set; }
            }

            public class Message
            {
                public string content { get; set; }
            }
        }
    }
}
