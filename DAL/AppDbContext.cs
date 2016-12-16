using System.Data.Entity;

using DAL.Entities;

namespace DAL
{
    public class AppDbContext : DbContext
    {

        public DbSet<Album> Album { get; set; }
        public DbSet<Artist> Artist { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<Playlist> Playlist { get; set; }
        public DbSet<Song> Song { get; set; }
        public DbSet<UserAccount> Users { get; set; }
        public DbSet<VipCodes> Codes { get; set; }

        public AppDbContext() : base("FinalDb")
        {
            InitializeDbContext();
        }

        public AppDbContext(string connectionString) : base(connectionString)
        {
            InitializeDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureMembershipRebootUserAccounts<UserAccount>();
        }

        private void InitializeDbContext()
        {
            //Database.SetInitializer(new AppDbInitializer());
            this.RegisterUserAccountChildTablesForDelete<UserAccount>();
        }
    }

}
