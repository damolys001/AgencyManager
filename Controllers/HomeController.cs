using DataFactory.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataFactory.Models;

namespace AgencyManager.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //var dd = DateTime.Now;
            ////var production=new TqHelper().GetNonLifeProduction("24-Feb-2019","25-Feb-2019");
            ////ModelState.AddModelError("Success", production.Count.ToString());
            //var msg = string.Empty;
            ////var fixedPayout = new AmHelper().GetFixedPayout(DateTime.Now, ref msg);
            ////var group = new TurnQuestHelper().GetGroupLifeProduction("01-Feb-2019", "25-Feb-2019");
            ////var individual = new TurnQuestHelper().GetIndividualLifeProduction("01-Feb-2019", "10-Feb-2019");
            //var prevNonLife = new TurnQuestHelper().GetNonLifeProduction("01-Jan-2019", "31-Jan-2019");
            //var prevLifelife = new TurnQuestHelper().GetLifeProductions("01-Jan-2019", "31-Jan-2019");

            //var non = new TurnQuestHelper().GetNonLifeProduction("01-Feb-2019", "28-Feb-2019");
            //var life = new TurnQuestHelper().GetLifeProductions("01-Feb-2019", "28-Feb-2019");
            //var payout =new AmHelper().GetQuickStartBenefit(prevLifelife, prevNonLife, life, non);
            //ModelState.AddModelError("Success", non.Count.ToString());
            var msg = string.Empty;
            var date = Convert.ToDateTime("30-Jun-2019");
            var agents = new TransHelper().GetAgents().ToList();
            //var agentAbove4Months = agents.Where(m => m.StartDate >= date && m.StartDate >= date.AddMonths(-4));
            var agentAbove4Months = agents.Where(m => m.StartDate <= date.AddMonths(-7));
            var aagents = agents.Where(m => m.StartDate <= date && m.StartDate >= date.AddMonths(-3));
            var ff = agentAbove4Months.Count();
            var ddd = ff;
            var fd = aagents.Count();
//var fixedPayout = new AmHelper().GetFixedPayout(agents, Convert.ToDateTime("30-Jun-2019"), ref msg);
            //var ss = fixedPayout.Count();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ComputePayout()
        {
            decimal _lifeQulifier = 15000;
            decimal _nonLifeQulifier = 10000;
            decimal _contestBenefitQualifier = 250000;
            decimal _slabValue = 50000;
            //decimal _contextBasePay = 15000;
            decimal _savingsQualifier = 100000;
            DateTime currentMonth=DateTime.Today.AddMonths(-1);
            var agents = new TransHelper().GetAgents().AsQueryable();
            var performanceBonusCategories= new ServiceManager<PerformanceBonusCategory>().GetAll().ToList();
            var msg = string.Empty;
            int days = DateTime.DaysInMonth(currentMonth.Year, currentMonth.Month);
            var startDate = "01-" + currentMonth.ToString("MMM-") + currentMonth.Year;
            var endDate = days + currentMonth.ToString("-MMM-") + currentMonth.Year;

            var non = new TurnQuestHelper().GetNonLifeProduction(startDate, endDate);
            var life = new TurnQuestHelper().GetLifeProductions(startDate, endDate);

            var previousMonth = currentMonth.AddMonths(-1);

            int numberOfDaysInPreviousMonth = DateTime.DaysInMonth(previousMonth.Year, previousMonth.Month);
            var prevStartDate = "01-" + previousMonth.ToString("MMM-") + previousMonth.Year;
            var prevEndDate = numberOfDaysInPreviousMonth + previousMonth.ToString("-MMM-") + previousMonth.Year;

            var prevNonLife = new TurnQuestHelper().GetNonLifeProduction(prevStartDate, prevEndDate);
            var prevLifelife = new TurnQuestHelper().GetLifeProductions(prevStartDate, prevEndDate);

            var payout = new AmHelper().GetQuickStartBenefit2(prevLifelife, prevNonLife, life, non);
            //Select Agents that are 4 moths ad above
            //var agentAbove4Months = agents.Where(m => m.StartDate <= currentMonth && m.StartDate <= currentMonth.AddMonths(-4));
            var agentAbove4Months = agents.Where(m => m.StartDate <= currentMonth.AddMonths(-4));
            //agents = agents.Where(m => m.StartDate <= productionDate && m.StartDate >= productionDate.AddMonths(-3));
            //var qualifiedForBonus =
            //    payout.FindAll(
            //        m => m.PreviousLifeProduction >= _lifeQulifier && m.PreviousNonLifeProduction >= _nonLifeQulifier);
            
            //Only Bonus
            var contest = payout.ToList();
            foreach (var agent in contest)
            {
                var production = payout.Find(m => m.AgentCode == agent.AgentCode);
                if (production != null)
                {
                    payout.Remove(production);
                    var multiplier = production.NonLifeProduction + production.LifeProduction;
                    var bonusCategory = performanceBonusCategories.Find(m => m.MinimumPremium <= multiplier && m.MaximumPremium >= multiplier);
                    if (bonusCategory != null)
                    {
                        production.Bonus = bonusCategory.Bonus;
                    }
                    payout.Add(production);
                }
            }

            foreach (var agent in agentAbove4Months)
            {
                var production= payout.Find(m => m.AgentCode == agent.AgentCode);
                if (production != null)
                {
                    payout.Remove(production);
                    if (production.PreviousLifeProduction >= _lifeQulifier && production.PreviousNonLifeProduction >= _nonLifeQulifier)
                    {
                        
                        //50% (NL) & 100% (Life) 
                        var nonLifeMultiplier = ((decimal) (50.0/100)*production.NonLifeProduction) +
                                                production.NonLifeProduction;
                        var lifeMultiplier = production.LifeProduction*2;
                        var multiplier = nonLifeMultiplier + lifeMultiplier;
                        var bonusCategory = performanceBonusCategories.Find(
                            m => m.MinimumPremium <= multiplier && m.MaximumPremium >= multiplier);
                        if (bonusCategory != null)
                        {
                            production.QuickStart =bonusCategory.Bonus;
                        }
                        
                    }
                    else
                    {
                        var multiplier = production.NonLifeProduction + production.LifeProduction;
                        var bonusCategory = performanceBonusCategories.Find(
                            m => m.MinimumPremium <= multiplier && m.MaximumPremium >= multiplier);
                        if (bonusCategory != null)
                        {
                            production.QuickStart = bonusCategory.Bonus;
                        }
                       
                    }
                    payout.Add(production);
                }
            }
            //ContestBenefit for production 
            var monthContextCategory= new ServiceManager<MonthlyContestCategory>().GetAll().ToList();
            //var contest = payout.FindAll(m => m.LifeProduction + m.NonLifeProduction >= 300000);
            
            foreach (var item in contest)
            {
                decimal contextBasePay = 0;
                decimal transactionAmount = item.LifeProduction + item.NonLifeProduction;
                var monthlyContest = monthContextCategory.Find(
                    m => m.MinimumPremium <= transactionAmount && m.MaximumPremium >= transactionAmount);
                if (monthlyContest != null)
                {
                    item.Contest = monthlyContest.Bonus;
                    payout.Remove(item);
                    //We need to lose the fraction from the first part of the equation
                    if (item.LifeProduction + item.NonLifeProduction >= 300000)
                    {
                        item.Contest = item.Contest + (((int)(item.LifeProduction + item.NonLifeProduction - _contestBenefitQualifier) / (int)_slabValue) * ((decimal)(10.0 / 100) * _slabValue));
                    }
                    payout.Add(item);
                }
               
                
            }
            //Add Savings
            var savingsPayout = payout.FindAll(m => m.LifeProduction + m.NonLifeProduction >= _savingsQualifier);
            foreach (var item in savingsPayout)
            {
                payout.Remove(item);
                item.Savings = 5000;
                payout.Add(item);
            }
            //Fixed Pay
            var fixedPayout = new AmHelper().GetFixedPayout(agents,Convert.ToDateTime(endDate), ref msg);
            foreach (var item in fixedPayout)
            {
                var alreadyExisting = payout.Find(m => m.AgentCode == item.AgentCode);
                if (alreadyExisting != null)
                {
                    payout.Remove(alreadyExisting);
                    alreadyExisting.FixedPay = item.FixedPay;
                    payout.Add(alreadyExisting);
                    continue;
                }
                //var payout = new Payout { AgentCode = item.agentCode, AgentName = item.agentName, NonLifeProduction = item.total };
                payout.Add(item);
            }
            return View(payout);
        }
    }
}