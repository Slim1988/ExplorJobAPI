using Slugify;

namespace ExplorJobAPI.Infrastructure.Slug
{
    public static class SlugBuilder
    {
        public static string Build(
            string value
        ) {
            var builder = new SlugHelper();
            return builder.GenerateSlug(value);
        }
    }
}
