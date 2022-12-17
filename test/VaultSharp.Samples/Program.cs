using System;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using VaultSharp.V1.AuthMethods;

namespace VaultSharp.Samples
{
    partial class Program
    {
        private const string ExpectedVaultVersion = "1.12.2";

        private static IVaultClient _unauthenticatedVaultClient;
        private static IVaultClient _authenticatedVaultClient;

        private static string _responseContent;

        public static void Main(string[] args)
        {
            const string path = "ProgramOutput.txt";

            using (var fs = new FileStream(path, FileMode.Create))
            {
                using (var sw = new StreamWriter(fs))
                {
                    Console.WriteLine();

                    Console.WriteLine("Please read HowToRunThisTestProgram.md for setup instructions.");
                    Console.WriteLine();
                    Console.WriteLine();

                    Console.Write("Writing results to file. Hang tight...");

                    var existingOut = Console.Out;
                    Console.SetOut(sw);

                    var settings = GetVaultClientSettings();
                    _unauthenticatedVaultClient = new VaultClient(settings);

                    _unauthenticatedVaultClient.V1.Auth.ResetVaultToken();

                    RunAllIntegrationTests();

                    Console.SetOut(existingOut);

                    Console.WriteLine();
                    Console.Write("I think we are done here. Press any key to exit...");
                }
            }

            Console.ReadLine();
        }

        private static void RunAllIntegrationTests()
        {
            RunSystemBackendSamples();
            RunAuthMethodSamples();
            RunSecretsEngineSamples();
        }

        private static VaultClientSettings GetVaultClientSettings(IAuthMethodInfo authMethodInfo = null)
        {
            var settings = new VaultClientSettings("http://localhost:8200", authMethodInfo)
            {
                AfterApiResponseAction = r =>
                {
                    var value = ((int)r.StatusCode + "-" + r.StatusCode) + "\n";
                    var content = r.Content != null ? r.Content.ReadAsStringAsync().Result : string.Empty;

                    _responseContent = "From Vault Server: " + value + content;

                    if (string.IsNullOrWhiteSpace(content))
                    {
                        Console.WriteLine(_responseContent);
                    }
                },
                Namespace = "bhjk",
                MyHttpClientProviderFunc = handler => new HttpClient(handler)
            };

            return settings;
        }
        
        private static void DisplayJson<T>(T value)
        {
            string line = "===========";

            var type = typeof(T);
            var genTypes = type.GenericTypeArguments;

            if (genTypes != null && genTypes.Length == 1)
            {
                var genType = genTypes[0];
                var subGenTypes = genType.GenericTypeArguments;

                // single generic. e.g. SecretsEngine<AuthBackend>
                if (subGenTypes == null || subGenTypes.Length == 0)
                {
                    Console.WriteLine(type.Name.Substring(0, type.Name.IndexOf('`')) + "<" + genType.Name + ">");
                }
                else
                {
                    // single sub-generic e.g. SecretsEngine<IEnumerable<AuthBackend>>
                    if (subGenTypes.Length == 1)
                    {
                        var subGenType = subGenTypes[0];

                        Console.WriteLine(type.Name.Substring(0, type.Name.IndexOf('`')) + "<" +
                                          genType.Name.Substring(0, genType.Name.IndexOf('`')) +
                                          "<" + subGenType.Name + ">>");
                    }
                    else
                    {
                        // double generic. e.g. SecretsEngine<Dictionary<string, AuthBackend>>
                        if (subGenTypes.Length == 2)
                        {
                            var subGenType1 = subGenTypes[0];
                            var subGenType2 = subGenTypes[1];

                            Console.WriteLine(type.Name.Substring(0, type.Name.IndexOf('`')) + "<" +
                                              genType.Name.Substring(0, genType.Name.IndexOf('`')) +
                                              "<" + subGenType1.Name + ", " + subGenType2.Name + ">>");
                        }
                    }
                }
            }
            else
            {
                // non-generic.
                Console.WriteLine(type.Name);
            }

            Console.WriteLine(line + line);
            Console.WriteLine(_responseContent);
            Console.WriteLine(JsonConvert.SerializeObject(value));
            Console.WriteLine(line + line);
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
