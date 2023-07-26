using System;
using System.Reflection;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace PFMBackendAPI.Helpers
{
	public class GetRules
	{
		public GetRules()
		{
		}


        public List<Rule> GetRulesList(string resourceName)
        {

            Assembly assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new Exception("File not found!");
                }

                using (StreamReader reader = new StreamReader(stream))
                {
                    string content = reader.ReadToEnd();
                    var deserializer = new DeserializerBuilder()
                        .WithNamingConvention(CamelCaseNamingConvention.Instance)
                        .Build();
                    List<Rule> rules = deserializer.Deserialize<List<Rule>>(content);
                    return rules;
                }

            }

        }
    }
}

