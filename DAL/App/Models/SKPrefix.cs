using System.Diagnostics.CodeAnalysis;

namespace DynamoDB.DAL.App.Models
{
    [ExcludeFromCodeCoverage]
    public static class SKPrefix
    {
        public const string USER = "USER|";
        public const string TITLE = "TITLE|";
        public const string SERVICE = "SERVICE|";
        public const string SERVICE_TITLE = "SERVICE_TITLE|";
        public const string USER_TITLE = "USER_TITLE|";

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
                case nameof(USER_TITLE):
                    return USER_TITLE;
                default:
                    return "";
            }
        }
    }
}
