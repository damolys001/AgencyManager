namespace DataFactory.Migrations
{
    using DataFactory.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Models.AgencyManagerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Models.AgencyManagerContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            var performanceBonusCategories= new List<PerformanceBonusCategory>
            {
                new PerformanceBonusCategory{PerformanceBonusCategoryId = 1,MinimumPremium = 20000,MaximumPremium = 50999, Bonus = 35000},
                new PerformanceBonusCategory{PerformanceBonusCategoryId = 2,MinimumPremium = 51000,MaximumPremium = 100999, Bonus = 40000},
                new PerformanceBonusCategory{PerformanceBonusCategoryId = 3,MinimumPremium = 101000,MaximumPremium = 150999, Bonus = 45000},
                new PerformanceBonusCategory{PerformanceBonusCategoryId = 4,MinimumPremium = 151000,MaximumPremium = 999999, Bonus = 50000},
            };
            var contestBonusCategories = new List<MonthlyContestCategory>
            {
                new MonthlyContestCategory{MonthlyContestCategoryId = 1,MinimumPremium = 51000,MaximumPremium = 100999, Bonus = 5000},
                new MonthlyContestCategory{MonthlyContestCategoryId = 2,MinimumPremium = 101000,MaximumPremium = 150999, Bonus = 7500},
                new MonthlyContestCategory{MonthlyContestCategoryId = 3,MinimumPremium = 151000,MaximumPremium = 200999, Bonus = 10000},
                new MonthlyContestCategory{MonthlyContestCategoryId = 4,MinimumPremium = 201000,MaximumPremium = 9999999, Bonus = 15000},
            };
            var salaries = new List<Salary>
            {
                new Salary{SalaryId=1,AgentType="Agent",Amount=35000},
                new Salary{SalaryId=2,AgentType="Supervisor",Amount=45000}
            };

            foreach (var item in salaries)
            {
                context.Salaries.AddOrUpdate(item);
            }
            foreach (var item in performanceBonusCategories)
            {
                context.PerformanceBonusCategories.AddOrUpdate(item);
            }
            foreach (var item in contestBonusCategories)
            {
                context.MonthlyContestCategories.AddOrUpdate(item);
            }
            context.SaveChanges();
        }
    }
}
