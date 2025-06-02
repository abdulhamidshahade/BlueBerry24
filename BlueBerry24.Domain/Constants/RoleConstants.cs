

namespace BlueBerry24.Domain.Constants
{
    public class RoleConstants
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string Admin = "Admin";
        public const string User = "User";

        public static readonly string[] AllRoles = { SuperAdmin, Admin, User };

        public static readonly string[] AdminRoles = { SuperAdmin, Admin };
        public static readonly string[] UserAndAbove = { SuperAdmin, Admin, User };
        public static readonly string[] AllIncludingGuest = { SuperAdmin, Admin, User };
    }
}
