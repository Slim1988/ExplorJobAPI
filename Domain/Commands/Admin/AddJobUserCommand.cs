using System.Collections.Generic;

namespace ExplorJobAPI.Domain.Commands.Admin
{
    public class AddJobUserCommand
    {
        public List<string> DomainIds { get; set; }
        public string Label { get; set; }
        public string Company { get; set; }
        public string Presentation { get; set; }
        public string Email { get; set; }
    }
}
