using System;
using Newtonsoft.Json;

namespace VaultSharp.Samples
{
    class Program
    {
        public static void Main(string[] args)
        {
            var settings = new VaultClientSettings();
            settings.VaultServerUriWithPort = "http://localhost:8200";

            settings.AfterApiResponseAction = r => Display(r.Content.ReadAsStringAsync().Result);

            IVaultClient vaultClient = new VaultClient(settings);

            var sealStatus = vaultClient.V1.System.GetSealStatusAsync().Result;
            Display(sealStatus);

            Console.ReadLine();
        }

        private static void Display<T>(T value)
        {
            Console.WriteLine(JsonConvert.SerializeObject(value));
        }
    }
}
