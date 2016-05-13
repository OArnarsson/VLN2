using Coder.Models;
using Coder.Models.Entity;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace Coder.Repositories
{
    public class UsersRepository
    {
        private readonly ApplicationDbContext db;

        public UsersRepository(ApplicationDbContext context)
        {
            db = context ?? new ApplicationDbContext();
        }

        public List<ApplicationUser> GetAllUsers()
        {
            return db.Users.ToList();
        }

        public ApplicationUser GetUserById(string id)
        {
            return db.Users.Find(id);
        }

        public void AddUser(ApplicationUser user)
        {
            db.Users.Add(user);
        }

        public void RemoveUser(ApplicationUser user)
        {
            db.Users.Remove(user);
        }

        public void UpdateState(EntityState state, ApplicationUser user)
        {
            db.Entry(user).State = state;
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }
    }
}