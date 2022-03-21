using System;
using System.Collections.Generic;

namespace ExplorJobAPI.Domain.Commands.KeywordLists
{
    public class KeywordListUpdateCommand
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Keywords { get; set; }

        public DateTime UpdatedOn {
            get { return DateTime.UtcNow; }
        }
    }
}
