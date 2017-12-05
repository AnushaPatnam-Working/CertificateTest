using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Jose;

namespace CertTesting
{
    class Program
    {
        static byte[] GetBytesFromPEM(string pemString, string section)
        {
            var header = String.Format("-----BEGIN {0}-----", section);
            var footer = String.Format("-----END {0}-----", section);

            var start = pemString.IndexOf(header, StringComparison.Ordinal);
            if (start < 0)
                return null;

            start += header.Length;
            var end = pemString.IndexOf(footer, start, StringComparison.Ordinal) - start;

            if (end < 0)
                return null;

            return Convert.FromBase64String(pemString.Substring(start, end));
        }
        static void Main(string[] args)
        {
            

            Dictionary<string, string> testData = new Dictionary<string, string>
            {
                { "test1", "test2" },
                { "test3", "test4" },
                { "test5", "test6" },
                { "test7", "test8" }
            };
            var cert = new X509Certificate2("MerlinLicenseCert10.p12", "password", X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet);
            var p = cert.PrivateKey as RSACryptoServiceProvider;
            RSACryptoServiceProvider newKey = new RSACryptoServiceProvider();
            newKey.ImportParameters(p.ExportParameters(true));
            

            string token = Jose.JWT.Encode(testData, newKey, JwsAlgorithm.RS256);
            

            var tmp = JWT.Decode<Dictionary<string, string>>(token, cert.PublicKey.Key, JwsAlgorithm.RS256);
            int i = 0;
        }
    }
}
