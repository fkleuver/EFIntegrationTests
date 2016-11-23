using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace EFIntegrationTests
{
    public class TodoContext : DbContext
    {
        public TodoContext() : base(nameof(TodoContext))
        {
            Configure();
        }

        public TodoContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            Configure();
        }

        public TodoContext(DbConnection connection, bool contextOwnsConnection = true) : base(connection, contextOwnsConnection)
        {
            Configure();
        }

        public DbSet<Todo> Todos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
        private void Configure()
        {
            Configuration.LazyLoadingEnabled = false;
        }

    }
}
