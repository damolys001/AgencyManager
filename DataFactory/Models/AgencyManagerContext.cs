namespace DataFactory.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class AgencyManagerContext : DbContext
    {
        // Your context has been configured to use a 'AgencyManagerContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'DataFactory.Models.AgencyManagerContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'AgencyManagerContext' 
        // connection string in the application configuration file.
        public AgencyManagerContext()
            : base("name=AgencyManagerContext")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Salary> Salaries { get; set; }
        public virtual DbSet<PerformanceBonusCategory> PerformanceBonusCategories { get; set; }
        public virtual DbSet<MonthlyContestCategory> MonthlyContestCategories { get; set; }

        public System.Data.Entity.DbSet<DataFactory.Models.Payout> Payouts { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}