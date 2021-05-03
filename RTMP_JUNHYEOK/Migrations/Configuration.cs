namespace RTMP_JUNHYEOK.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<RTMP_JUNHYEOK.Models.ChatEntities1>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "RTMP_JUNHYEOK.Models.Entities";
        }

        protected override void Seed(RTMP_JUNHYEOK.Models.ChatEntities1 context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
