namespace ExplorJobAPI.Domain.Models.Localization
{
    public class Coordinates
    {
        public string MapEmbedUrl { get; set; }

        public Coordinates(
            string mapEmbedUrl
        ) {
            MapEmbedUrl = mapEmbedUrl;
        }
    }
}
