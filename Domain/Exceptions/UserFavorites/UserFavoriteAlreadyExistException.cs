using System;
using Serilog;

namespace ExplorJobAPI.Domain.Exceptions.UserFavorites
{
    public class UserFavoriteAlreadyExistException : Exception
    {
        public UserFavoriteAlreadyExistException() {
            Log.Error(this.ToString());
        }

        public UserFavoriteAlreadyExistException(
            string message
        ) : base (message) {
            Log.Error(this.ToString());
        }
    }
}
