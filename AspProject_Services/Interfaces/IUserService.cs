using AspProject_Entities.Models;
using System.Collections.Generic;

namespace AspProject_Services.Interfaces
{
    public interface IUserService
    {
        bool CheckIfUserExists(string username);
        void AddUser(User user);
        string GetUserDetails(string username, string password);
        bool GetUser(string username, string password, out User user);
        User GetUser(string username, string password);
        bool CheckIfPasswordMatch(string username, string password);
        void UpdateUser(User user);
    }
}
