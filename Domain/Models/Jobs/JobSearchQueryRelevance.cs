namespace ExplorJobAPI.Domain.Models.Jobs
{
    public static class JobSearchQueryRelevance
    {
        
        public static int QueryOnJobLabel = 15;
        public static int QueryOnCompany = 12;
        public static int QueryOnLocalisation = 10;
        public static int QueryOnJobPresentation = 1;
        public static int QueryOnUserPresentation = 1;
        public static double SimilarityOnJobLabel = 1.5;
        public static double SimilarityOnCompany = 1.2;
        public static double SimilarityOnLocalisation = 1;
        public static double SimilarityOnJobPresentation = 0.1;
        public static double SimilarityOnUserPresentation = 0.1;
        public static int JobLabel = 5;
        public static int JobDomain = 2;
        public static int Company = 4;
        public static int Localisation = 3;
        public static int JobPresentation = 1;
        public static int UserPresentation = 1;

        public static int MinimumRootThreshold = 4;

        public static double LowPriorityNounIndex = 0.2;
        public static double NormalPriorityNounIndex = 1;
        public const int LowPriorityMinimum = 1;
        public const double LowPriorityNounRelevanceRate = 0.2;
    }
}
