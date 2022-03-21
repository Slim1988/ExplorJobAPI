using System;
using System.Collections.Generic;

namespace ExplorJobAPI.Domain.Commands.Agglomerations
{
    public class AgglomerationUpdateCommand
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public List<string> ZipCodes { get; set; }

        public DateTime UpdatedOn {
            get { return DateTime.UtcNow; }
        }
    }
}
