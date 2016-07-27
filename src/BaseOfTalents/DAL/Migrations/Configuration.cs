using System.Data.Entity.Migrations;

namespace DAL.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<BOTContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

    }
}