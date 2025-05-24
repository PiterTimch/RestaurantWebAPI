namespace RestaurantWebAPI.Constants
{
    public class Roles
    {
        public static string Admin => "Admin";
        public static string User => "User";

        public static string[] AllRoles => new[] { Admin, User };
    }
}
