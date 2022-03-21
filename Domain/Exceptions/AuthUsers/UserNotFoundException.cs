using System;
using Serilog;

namespace ExplorJobAPI.Domain.Exceptions.AuthUsers
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException() {
            Log.Error(this.ToString());
        }

        public UserNotFoundException(
            string message
        ) : base (message) {
            Log.Error(this.ToString());
        }
    }
}
