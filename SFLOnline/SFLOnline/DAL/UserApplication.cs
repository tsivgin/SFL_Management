using SFLOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SFLOnline.DAL
{
    public class UserApplication
    {
        UserService userRepo = new UserService();
        public Person GetByUsernameAndPassword(Person user)
        {
            return userRepo.GetByUsernameAndPassword(user);
        }

        public Person ValidateUser(Person user)
        {
            return userRepo.ValidateUser(user);
        }

        public void ChangePassword(Person user)
        {
            userRepo.ChangePassword(user);
        }
    }
}