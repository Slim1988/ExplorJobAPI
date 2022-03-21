using System;
using System.Collections.Generic;

namespace ExplorJobAPI.Domain.Commands.KeywordLists
{
    public class KeywordListCreateCommand
    {
        public string Name { get; set; }
        public List<string> Keywords { get; set; }

        public DateTime CreatedOn {
            get { return DateTime.UtcNow; }
        }

        public DateTime UpdatedOn {
            get { return DateTime.UtcNow; }
        }
    }
}
