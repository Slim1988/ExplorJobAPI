namespace ExplorJobAPI.Domain.Dto.UserReporting
{
    public class UserReportingReasonDto
    {
        public string Id { get; set; }
        public string Label { get; set; }

        public UserReportingReasonDto(
            string id,
            string label
        ) {
            Id = id;
            Label = label;
        }
    }
}
