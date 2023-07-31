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


        /// <summary>
        /// Retrieves a list of Rule objects from a yaml file.
        /// </summary>
        /// <param name="resourceName">The name of the yaml file containing the rules.</param>
        /// <returns>A List of Rule objects representing the rules defined in the file.</returns>
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

