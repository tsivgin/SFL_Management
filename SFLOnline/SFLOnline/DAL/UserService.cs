using SFLOnline.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SFLOnline.DAL
{
    public class UserService
    {
        private SchoolContext db = new SchoolContext();

        public Person GetByUsernameAndPassword(Person user)
        {
            return db.Persons.Where(u => u.EMail == user.EMail & u.Password == user.Password).FirstOrDefault();
        }

        public Person ValidateUser(Person user)
        {
            return db.Persons.Where(u => u.EMail == user.EMail).FirstOrDefault();
        }

        public void ChangePassword(Person user)
        {
            var isValidated = ValidateUser(user);
            if (isValidated != null)
            {
                isValidated.Password = "sflonlinerocks";
                db.Entry(isValidated).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}