namespace Dogstagram.Server.Data
{
    public class Validation
    {
        public class Dog
        {
            public const int MaxDescriptionLength = 2000;
        }

        public class User
        {
            public const int MaxNameLength = 40;
            public const int MaxBiographyLength = 150;
        }
    }
}
