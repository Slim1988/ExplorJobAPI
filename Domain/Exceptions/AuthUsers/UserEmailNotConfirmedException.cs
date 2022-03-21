using System;
using Serilog;

namespace ExplorJobAPI.Domain.Exceptions.AuthUsers
{
    public class UserEmailNotConfirmedException : Exception
    {
        public UserEmailNotConfirmedException() {
            Log.Error(this.ToString());
        }

        public UserEmailNotConfirmedException(
            string message
        ) : base (message) {
            Log.Error(this.ToString());
        }
    }
}
