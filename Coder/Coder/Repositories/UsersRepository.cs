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

        /*
        * Initialization.
        */
        public UsersRepository(ApplicationDbContext context)
        {
            db = context ?? new ApplicationDbContext();
        }

        /*
        * Fetches all users from the database.
        */
        public List<ApplicationUser> GetAllUsers()
        {
            return db.Users.ToList();
        }

        /*
        * Fetches a user with a specific ID.
        */
        public ApplicationUser GetUserById(string id)
        {
            return db.Users.Find(id);
        }

        /*
        * Adds a user to the database.
        */
        public void AddUser(ApplicationUser user)
        {
            db.Users.Add(user);
        }

        /*
        * Removes a user from the database.
        */
        public void RemoveUser(ApplicationUser user)
        {
            db.Users.Remove(user);
        }

        /*
        * Updates user in the database.
        */
        public void UpdateState(EntityState state, ApplicationUser user)
        {
            db.Entry(user).State = state;
        }

        /*
        * Saves changes to the database.
        */
        public void SaveChanges()
        {
            db.SaveChanges();
        }
    }
}