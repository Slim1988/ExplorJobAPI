namespace ExplorJobAPI.Domain.Models.Offers
{
    public class Promote
    {
        public string CompanyName { get; set; }
        public string Slug { get; set; }
        public string LogoUrl { get; set; }
        public string HighlightMessage { get; set; }
        public string Message { get; set; }
        public bool IsResultHeaderLogoActive { get; set; }
        public bool IsResultLineLogoActive { get; set; }
        public bool IsContactModalLogoActive { get; set; }

        public Promote(
            string companyName,
            string slug,
            string logoUrl,
            string highlightMessage,
            string message,
            bool isResultHeaderLogoActive,
            bool isContactModalLogoActive
        ) {
            CompanyName = companyName;
            Slug = slug;
            LogoUrl = logoUrl;
            HighlightMessage = highlightMessage;
            Message = message;
            IsResultLineLogoActive = true;
            IsResultHeaderLogoActive = isResultHeaderLogoActive;
            IsContactModalLogoActive = isContactModalLogoActive;
        }

        public Promote(
            string companyName,
            string slug,
            string logoUrl
        ) {
            CompanyName = companyName;
            Slug = slug;
            LogoUrl = logoUrl;
            HighlightMessage = null;
            Message = null;
            IsResultLineLogoActive = true;
            IsResultHeaderLogoActive = false;
            IsContactModalLogoActive = false;
        }

        public Promote() {
            CompanyName = null;
            Slug = null;
            LogoUrl = null;
            HighlightMessage = null;
            Message = null;
            IsResultHeaderLogoActive = false;
            IsResultLineLogoActive = false;
            IsContactModalLogoActive = false;
        }

        public bool IsEmpty() {
            return string.IsNullOrEmpty(CompanyName)
                && string.IsNullOrEmpty(Slug)
                && string.IsNullOrEmpty(LogoUrl)
                && string.IsNullOrEmpty(HighlightMessage)
                && string.IsNullOrEmpty(Message)
                && IsResultHeaderLogoActive == false
                && IsResultLineLogoActive == false
                && IsContactModalLogoActive == false;
        }
    }
}
