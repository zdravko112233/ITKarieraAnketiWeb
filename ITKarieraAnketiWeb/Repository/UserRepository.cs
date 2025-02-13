using System;
using System.Linq;
using ITKarieraAnketiWeb.Database;
using Microsoft.EntityFrameworkCore;
using ITKarieraAnketiWeb.Database;
using ITKarieraAnketiWeb.Models; 

public class UserRepository
{
    private readonly AppDbContext _context; 
    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}