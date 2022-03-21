using System;
using System.Collections.Generic;

namespace ExplorJobAPI.Domain.Models.KeywordLists
{
    public class KeywordList
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<string> Keywords { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public KeywordList(
            Guid id,
            string name,
            List<string> keywords,
            DateTime createdOn,
            DateTime updatedOn
        ) {
            Id = id;
            Name = name;
            Keywords = keywords;
            CreatedOn = createdOn;
            UpdatedOn = updatedOn;
        }
    }
}
