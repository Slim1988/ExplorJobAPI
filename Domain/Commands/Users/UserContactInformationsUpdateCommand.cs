using System;
using System.Collections.Generic;

namespace ExplorJobAPI.Domain.Commands.Users
{
    public class UserContactInformationsUpdateCommand
    {
        public string Id { get; set; }
        public string Phone { get; set; }
        public UserAddressUpdateCommand Address { get; set; }
        public List<string> ContactMethodIds { get; set; }
        public DateTime UpdatedOn {
            get { return DateTime.UtcNow; }
        }
    }
}
