using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestTaskGeekForLess.Models;

namespace TestTaskGeekForLess.Data
{
    public class TestTaskGeekForLessContext : DbContext
    {
        public TestTaskGeekForLessContext (DbContextOptions<TestTaskGeekForLessContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=ANDREW\\SQLEXPRESS;Database=config_tree;TrustServerCertificate=True;Trusted_Connection=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TreeNode>()
                .Property(b => b.Value);
            modelBuilder.Entity<TreeNode>()
                .Property(b => b.Name);
            modelBuilder.Entity<TreeNode>()
                .Property(b => b.ParentId);
        }

        public DbSet<TestTaskGeekForLess.Models.TreeNode> TreeNode { get; set; } = default!;
    }
}
