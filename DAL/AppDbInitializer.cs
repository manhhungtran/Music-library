using System.Data.Entity;

namespace DAL
{
    public class AppDbInitializer : DropCreateDatabaseAlways<AppDbContext>
    {
        public override void InitializeDatabase(AppDbContext context)
        {
            base.InitializeDatabase(context);

            // Created for future...

            context.SaveChanges();
        }
    }
}