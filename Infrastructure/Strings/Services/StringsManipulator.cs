namespace ExplorJobAPI.Infrastructure.Strings.Services
{
    public class StringsManipulator
    {
        public StringsManipulator() { }

        public string ReplaceLastOccurrence(
            string source,
            string find,
            string replace
        ) {
            int place = source.LastIndexOf(
                find
            );

            return place > -1
                ? source
                    .Remove(place, find.Length)
                    .Insert(place, replace)
                : source;
        }
    }
}
