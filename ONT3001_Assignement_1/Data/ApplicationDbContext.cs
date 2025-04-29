using Microsoft.EntityFrameworkCore;
using ONT3001_Assignement_1.Models;

namespace ONT3001_Assignement_1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) 
        {
        
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
    }
}
