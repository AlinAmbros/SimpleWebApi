﻿using Microsoft.EntityFrameworkCore;

namespace MinimalApi
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Contact> Contacts { get; set; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
