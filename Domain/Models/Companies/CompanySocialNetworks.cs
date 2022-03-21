namespace ExplorJobAPI.Domain.Models.Companies
{
    public class CompanySocialNetworks
    {
        public string FacebookUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string InstagramUrl { get; set; }
        public string LinkedInUrl { get; set; }
        public string YoutubeUrl { get; set; }
        
        public CompanySocialNetworks(
            string facebookUrl,
            string twitterUrl,
            string instagramUrl,
            string linkedInUrl,
            string youtubeUrl
        ) {
            FacebookUrl = facebookUrl;
            TwitterUrl = twitterUrl;
            InstagramUrl = instagramUrl;
            LinkedInUrl = linkedInUrl;
            YoutubeUrl = youtubeUrl;
        }
    }
}
