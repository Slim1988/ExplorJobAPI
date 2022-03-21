using System.Text;
using Microsoft.AspNetCore.WebUtilities;

namespace ExplorJobAPI.Infrastructure.Http.Services
{
    public class UrlEncoder
    {
        public string Encode(
            string urlValue
        ) {
            return WebEncoders.Base64UrlEncode(
                Encoding.UTF8.GetBytes(
                    urlValue
                )
            );
        }

        public string Decode(
            string urlValue
        ) {
            return Encoding.UTF8.GetString(
                WebEncoders.Base64UrlDecode(
                    urlValue
                )
            );
        }
    }
}
