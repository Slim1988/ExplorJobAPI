namespace ExplorJobAPI.Domain.Models.Companies
{
    public class CompanyDynamicField
    {
        public string Title { get; set; }
        public string Content { get; set; }

        public CompanyDynamicField(
            string title,
            string content
        ) {
            Title = title;
            Content = content;
        }
    }
}
