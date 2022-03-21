using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplorJobAPI.Domain.Models.AuthTokens
{
    public class AuthTokenEmployee
    {
        public string AccessToken { get; set; }
        public DateTime Expiration { get; set; }
        
        public AuthTokenEmployee(
            // TODO
        ) {
            // TODO  
        }
    }
}
