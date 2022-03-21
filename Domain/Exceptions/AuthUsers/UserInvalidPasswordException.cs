using System;
using Serilog;

namespace ExplorJobAPI.Domain.Exceptions.AuthUsers
{
    public class UserInvalidPasswordException : Exception
    { 
        public UserInvalidPasswordException() {
            Log.Error(this.ToString());
        }

        public UserInvalidPasswordException(
            string message
        ) : base (message) {
            Log.Error(this.ToString());
        }
    }
}
