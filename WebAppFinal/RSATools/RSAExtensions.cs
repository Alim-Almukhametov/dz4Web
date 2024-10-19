using System.Security.Cryptography;

namespace WebAppFinal.RSATools
{
    public class RSAExtensions
    {
        public static RSA GetPrivateKey()
        {
            //var key = File.ReadAllText(@"..\private_key.pem");
            var key = File.ReadAllText("private_key.pem");
            var rsa = RSA.Create();

            rsa.ImportFromPem(key);
            return rsa;
        }
    }
}
