using System;
using System.Linq;
using ITKarieraAnketiWeb.Database;
using ITKarieraAnketiWeb.Entities;
using Microsoft.EntityFrameworkCore;
using ITKarieraAnketiWeb.Database;

namespace ITKarieraAnketiWeb.Repository
{
    public class UserRepository
    {
        private readonly DbContext _context;
        public UserRepository(DbContext context)
        {
            _context = context;
        }
        public void AddUser(User user)
        {
            _context.Add(user);
            _context.SaveChanges();
        }
    }
}
