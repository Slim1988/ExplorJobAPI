namespace ExplorJobAPI.Domain.Models.Companies
{
    public class CompanyTexts
    {
        public string CompanyCatchPhrase { get; set; }
        public string Presentation { get; set; }
        public string ProfilesSought { get; set; }
        public string JobsCatchPhrase { get; set; }
        public string DynamicText1Title { get; set; }
        public string DynamicText1Content { get; set; }
        public string DynamicText2Title { get; set; }
        public string DynamicText2Content { get; set; }
        public string DynamicText3Title { get; set; }
        public string DynamicText3Content { get; set; }
        public string FooterPhrase { get; set; }
        public string CareerWebsiteWording { get; set; }

        public CompanyTexts(
            string companyCatchPhrase,
            string presentation,
            string profilesSought,
            string jobsCatchPhrase,
            string dynamicText1Title,
            string dynamicText1Content,
            string dynamicText2Title,
            string dynamicText2Content,
            string dynamicText3Title,
            string dynamicText3Content,
            string footerPhrase,
            string careerWebsiteWording
        ) {
            CompanyCatchPhrase = companyCatchPhrase;
            Presentation = presentation;
            ProfilesSought = profilesSought;
            JobsCatchPhrase = jobsCatchPhrase;
            DynamicText1Title = dynamicText1Title;
            DynamicText1Content = dynamicText1Content;
            DynamicText2Title = dynamicText2Title;
            DynamicText2Content = dynamicText2Content;
            DynamicText3Title = dynamicText3Title;
            DynamicText3Content = dynamicText3Content;
            FooterPhrase = footerPhrase;
            CareerWebsiteWording = careerWebsiteWording;
        }
    }
}
