using System.Collections.Generic;

namespace ExplorJobAPI.Domain.Models.Companies
{
    public class CompanyMedias
    {
        public string LogoUrl { get; set; }
        public string BannerUrl { get; set; }
        public List<string> PhotoUrls { get; set; }
        public string YoutubeVideoEmbedUrl { get; set; }
        public string FooterPhotoUrl { get; set; }

        public CompanyMedias(
            string logoUrl,
            string bannerUrl,
            List<string> photoUrls,
            string youtubeVideoEmbedUrl,
            string footerPhotoUrl
        ) {
            LogoUrl = logoUrl;
            BannerUrl = bannerUrl;
            PhotoUrls = photoUrls;
            YoutubeVideoEmbedUrl = youtubeVideoEmbedUrl;
            FooterPhotoUrl = footerPhotoUrl;
        }
    }
}
