namespace ExplorJobAPI.Domain.Dto.Jobs
{
    public class JobDomainDto
    {
        public string Id { get; set; }
        public string Label { get; set; }

        public JobDomainDto(
            string id,
            string label
        ) {
            Id = id;
            Label = label;
        }
    }
}
