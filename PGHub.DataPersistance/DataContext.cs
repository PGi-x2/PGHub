using Microsoft.EntityFrameworkCore;
using PGHub.Domain.Entities;

namespace PGHub.DataPersistance
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
               // This is an empty block because the primary scope of this constructor is to pass the options paramerter to the base class constructor (DbContext) 
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
    }
}
