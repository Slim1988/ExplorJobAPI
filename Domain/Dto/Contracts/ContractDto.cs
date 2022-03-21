using System;

namespace ExplorJobAPI.Domain.Dto.Contracts
{
    public class ContractDto
    {
        public string Id { get; set; }
        public string Context { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Content { get; set; }
        public string ContentHTML { get; set; }
        public DateTime PublishedOn { get; set; }

        public ContractDto(
            string id,
            string context,
            string name,
            string version,
            string content,
            string contentHTML,
            DateTime publishedOn
        ) {
            Id = id;
            Context = context;
            Name = name;
            Version = version;
            Content = content;
            ContentHTML = contentHTML;
            PublishedOn = publishedOn;
        }
    }
}
