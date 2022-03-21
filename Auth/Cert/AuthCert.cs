using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace ExplorJobAPI.Auth.Cert
{
    public static class AuthCert
    {
        public static X509Certificate2 Get() {
            return new X509Certificate2(
                Path.GetFullPath("Auth/Cert/cert.pfx"),
                Config.CertPassword
            );
        }
    }
}
