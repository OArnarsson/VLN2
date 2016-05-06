using Coder.Models;
using Coder.Models.Entity;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
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
    }
}