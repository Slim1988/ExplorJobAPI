using System;
using System.Collections.Generic;

namespace ExplorJobAPI.Domain.Commands.Agglomerations
{
    public class AgglomerationCreateCommand
    {
        public string Label { get; set; }
        public List<string> ZipCodes { get; set; }

        public DateTime CreatedOn {
            get { return DateTime.UtcNow; }
        }

        public DateTime UpdatedOn {
            get { return DateTime.UtcNow; }
        }
    }
}
