using System;
using ExplorJobAPI.Domain.Dto.Contracts;

namespace ExplorJobAPI.Domain.Models.Contracts
{
    public class Contract
    {
        public Guid Id { get; set; }
        public string Context { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Content { get; set; }
        public string ContentHTML { get; set; }
        public DateTime PublishedOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public Contract(
            Guid id,
            string context,
            string name,
            string version,
            string content,
            string contentHTML,
            DateTime publishedOn,
            DateTime createdOn,
            DateTime updatedOn
        ) {
            Id = id;
            Context = context;
            Name = name;
            Version = version;
            Content = content;
            ContentHTML = contentHTML;
            PublishedOn = publishedOn;
            CreatedOn = createdOn;
            UpdatedOn = updatedOn;
        }

        public ContractDto ToDto() {
            return new ContractDto(
                Id.ToString(),
                Context,
                Name,
                Version,
                Content,
                ContentHTML,
                PublishedOn
            );
        }
    }
}
