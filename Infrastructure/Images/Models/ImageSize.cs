namespace ExplorJobAPI.Infrastructure.Images.Models
{
    public class ImageSize
    {
        public string Label { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public ImageSize() { }

        public ImageSize(
            string label,
            int width,
            int height
        ) {
            Label = label;
            Width = width;
            Height = height;
        }
    }
}
