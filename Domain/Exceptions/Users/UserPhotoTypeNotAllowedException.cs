using System;
using Serilog;

namespace ExplorJobAPI.Domain.Exceptions.Users
{

    public class UserPhotoTypeNotAllowedException : Exception
    {
        public UserPhotoTypeNotAllowedException() {
            Log.Error(this.ToString());
        }

        public UserPhotoTypeNotAllowedException(
            string message
        ) : base (message) {
            Log.Error(this.ToString());
        }
    }
}
