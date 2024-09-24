using Azure.Identity;
using Student_Management.Models;

namespace Student_Management.Data
{
    public class UserConstants
    {
        public static List<UserModel> Users = new List<UserModel>()
        {
            new UserModel() { Username = "jason_admin", EmailAddress = "jason.admin@gmail.com",
                Password = "admin123", GivenName = "Jason", Surname = "Bryant",Role = "Administrator" },
            new UserModel() { Username = "alex_student", EmailAddress = "alex.seller@gmail.com",
                Password = "user456", GivenName = "Alex", Surname = "Lambert",Role = "Student" },
        };

    }
}
