namespace DynamoDB.DAL.App.Models
{
    public static class SKPrefix
    {
        public const string USER = "USER|";
        public const string TITLE = "TITLE|";
        public const string SERVICE = "SERVICE|";
        public const string SERVICE_TITLE = "SERVICE_TITLE|";

        public static string GetSK(Type type)
        {
            switch(type.Name)
            {
                case nameof(User):
                    return USER;
                case nameof(Title):
                    return TITLE;
                case nameof(SERVICE):
                    return SERVICE;
                case nameof(SERVICE_TITLE):
                    return SERVICE_TITLE;
                default:
                    return "";
            }
        }
    }
}
