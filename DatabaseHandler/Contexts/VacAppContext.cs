using DatabaseHandler.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseHandler.Contexts
{
    public class VacAppContext : DbContext
    {
        public DbSet<VacApplication> VacApplications { get; set; }
        public DbSet<Employee> Employees { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-PD5TVHT; Initial Catalog=VacationApplicationsDB; Integrated Security=True;");
        }
    }
}
