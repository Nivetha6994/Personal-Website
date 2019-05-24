using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project1.Models;

namespace Project1.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Academic> Academics { get; set; }
        public DbSet<Professional> Professionals { get; set; }

        public DbSet<Recruiter> Recruiters { get; set; }

        public DbSet<FileData> FilesData { get; set; }
    }
}
