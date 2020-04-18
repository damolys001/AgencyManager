using DataFactory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFactory.Services
{
    public class AmHelper
    {
        private decimal _savings = 5000;
        private decimal _lifeQulifier = 15000;
        private decimal _nonLifeQulifier = 10000;
        public List<Payout> GetQuickStartBenefit2(List<Production> previousLifeProduction, List<Production> previousNonLifeProduction, List<Production> currentLifeProduction, List<Production> currentNonLifeProduction)
        {
            var payoutList = new List<Payout>();
            //Must have done (Life ₦15,000 & Non-Life ₦10,000) in previous month
            //previousNonLifeProduction = previousNonLifeProduction.FindAll(m => m.TransactionType.Equals("NB", StringComparison.InvariantCultureIgnoreCase));
            //previousLifeProduction = previousLifeProduction.FindAll(m => m.TransactionType.Equals("NEW BUSINESS", StringComparison.InvariantCultureIgnoreCase));
            var previousNonLifeData = previousNonLifeProduction.Select(k => new { k.AgentCode,k.AgentName, k.Premium }).GroupBy(x => new { x.AgentCode,x.AgentName }, (key, group) => new
            {
                agentCode = key.AgentCode,
                agentName=key.AgentName,
                transactionCount = group.Count(),
                total = group.Sum(k => k.Premium)
            }).ToList();
            var previousLifeData = previousLifeProduction.Select(k => new { k.AgentCode,k.AgentName, k.Premium }).GroupBy(x => new { x.AgentCode, x.AgentName }, (key, group) => new
            {
                agentCode = key.AgentCode,
                agentName=key.AgentName,
                transactionCount = group.Count(),
                total = group.Sum(k => k.Premium)
            }).ToList();
            //50% (NL) & 100% (Life) 
            //filter New Business
            //currentNonLifeProduction = currentNonLifeProduction.FindAll(m => m.TransactionType.Equals("NB", StringComparison.InvariantCultureIgnoreCase));
            //currentLifeProduction = currentLifeProduction.FindAll(m => m.TransactionType.Equals("NEW BUSINESS", StringComparison.InvariantCultureIgnoreCase));
            
            //Group Transactions by Agent
            var nonLifeData = currentNonLifeProduction.Select(k => new { k.AgentCode,k.AgentName, k.Premium }).GroupBy(x => new { x.AgentCode, x.AgentName }, (key, group) => new
            {
                agentCode = key.AgentCode,
                agentName=key.AgentName,
                transactionCount = group.Count(),
                total = group.Sum(k => k.Premium)
            }).ToList();
            var lifeData = currentLifeProduction.Select(k => new { k.AgentCode, k.AgentName, k.Premium }).GroupBy(x => new { x.AgentCode, x.AgentName }, (key, group) => new
            {
                agentCode = key.AgentCode,
                agentName = key.AgentName,
                transactionCount = group.Count(),
                total = group.Sum(k => k.Premium)
            }).ToList();
            foreach (var item in previousLifeData)
            {
                var alreadyExisting = payoutList.Find(m => m.AgentCode == item.agentCode);
                if (alreadyExisting != null)
                {
                    payoutList.Remove(alreadyExisting);
                    alreadyExisting.PreviousLifeProduction = item.total;
                    payoutList.Add(alreadyExisting);
                    continue;
                }
                var payout = new Payout { AgentCode = item.agentCode, AgentName = item.agentName, PreviousLifeProduction = item.total };
                payoutList.Add(payout);
            }
            foreach (var item in previousNonLifeData)
            {
                var alreadyExisting = payoutList.Find(m => m.AgentCode == item.agentCode);
                if (alreadyExisting != null)
                {
                    payoutList.Remove(alreadyExisting);
                    alreadyExisting.PreviousNonLifeProduction = item.total;
                    payoutList.Add(alreadyExisting);
                    continue;
                }
                var payout = new Payout { AgentCode = item.agentCode, AgentName = item.agentName, PreviousNonLifeProduction = item.total };
                payoutList.Add(payout);
            }
            foreach (var item in lifeData)
            {
                var alreadyExisting = payoutList.Find(m => m.AgentCode == item.agentCode);
                if (alreadyExisting != null)
                {
                    payoutList.Remove(alreadyExisting);
                    alreadyExisting.LifeProduction = item.total;
                    payoutList.Add(alreadyExisting);
                    continue;
                }
                var payout = new Payout { AgentCode = item.agentCode, AgentName = item.agentName, LifeProduction = item.total };
                payoutList.Add(payout);
            }
            foreach (var item in nonLifeData)
            {
                var alreadyExisting = payoutList.Find(m => m.AgentCode == item.agentCode);
                if (alreadyExisting != null)
                {
                    payoutList.Remove(alreadyExisting);
                    alreadyExisting.NonLifeProduction = item.total;
                    payoutList.Add(alreadyExisting);
                    continue;
                }
                var payout = new Payout { AgentCode = item.agentCode, AgentName = item.agentName, NonLifeProduction = item.total };
                payoutList.Add(payout);
            }
            return payoutList;
        }
        public List<Payout> GetQuickStartBenefit_Old(List<Production> previousLifeProduction, List<Production> previousNonLifeProduction, List<Production> currentLifeProduction, List<Production> currentNonLifeProduction)
        {
            var performanceBonusCategories= new ServiceManager<PerformanceBonusCategory>().GetAll().ToList();
            var payoutList = new List<Payout>();
            //Must have done (Life ₦15,000 & Non-Life ₦10,000) in previous month
            //previousNonLifeProduction = previousNonLifeProduction.FindAll(m => m.TransactionType.Equals("NB", StringComparison.InvariantCultureIgnoreCase));
            //previousLifeProduction = previousLifeProduction.FindAll(m => m.TransactionType.Equals("NEW BUSINESS", StringComparison.InvariantCultureIgnoreCase));
            var previousNonLifeData = previousNonLifeProduction.Select(k => new { k.AgentCode,k.AgentName, k.Premium }).GroupBy(x => new { x.AgentCode,x.AgentName }, (key, group) => new
            {
                agentCode = key.AgentCode,
                agentName=key.AgentName,
                transactionCount = group.Count(),
                total = group.Sum(k => k.Premium)
            }).ToList();
            var previousLifeData = previousLifeProduction.Select(k => new { k.AgentCode,k.AgentName, k.Premium }).GroupBy(x => new { x.AgentCode, x.AgentName }, (key, group) => new
            {
                agentCode = key.AgentCode,
                agentName=key.AgentName,
                transactionCount = group.Count(),
                total = group.Sum(k => k.Premium)
            }).ToList();
            //50% (NL) & 100% (Life) 
            //filter New Business
            //currentNonLifeProduction = currentNonLifeProduction.FindAll(m => m.TransactionType.Equals("NB", StringComparison.InvariantCultureIgnoreCase));
            //currentLifeProduction = currentLifeProduction.FindAll(m => m.TransactionType.Equals("NEW BUSINESS", StringComparison.InvariantCultureIgnoreCase));
            
            //Group Transactions by Agent
            var nonLifeData = currentNonLifeProduction.Select(k => new { k.AgentCode,k.AgentName, k.Premium }).GroupBy(x => new { x.AgentCode, x.AgentName }, (key, group) => new
            {
                agentCode = key.AgentCode,
                agentName=key.AgentName,
                transactionCount = group.Count(),
                total = group.Sum(k => k.Premium)
            }).ToList();
            var lifeData = currentLifeProduction.Select(k => new { k.AgentCode, k.AgentName, k.Premium }).GroupBy(x => new { x.AgentCode, x.AgentName }, (key, group) => new
            {
                agentCode = key.AgentCode,
                agentName = key.AgentName,
                transactionCount = group.Count(),
                total = group.Sum(k => k.Premium)
            }).ToList();
            #region Remove Users without Production for the month
            //Remove Users without Production for the month
            var distLife = from list1Item in previousLifeData
                       join list2Item in lifeData on list1Item.agentCode equals list2Item.agentCode
                       where (list2Item != null)
                       select list1Item;
            var distNonLife = from list1Item in previousNonLifeData
                           join list2Item in nonLifeData on list1Item.agentCode equals list2Item.agentCode
                           where (list2Item != null)
                           select list1Item;
            
            #endregion
           
            //Remove Under Performers
            #region Remove Under Performers
            foreach (var item in previousNonLifeData)
            {
                var payout = new Payout { AgentCode = item.agentCode, AgentName = item.agentName, PreviousNonLifeProduction = item.total };
                
                if (item.total < _nonLifeQulifier)
                {
                    var nonLifeTransactions = nonLifeData.Find(m => m.agentCode == item.agentCode);
                    var lifeTransactions = lifeData.Find(m => m.agentCode == item.agentCode);
                    if (nonLifeTransactions != null)
                    {
                        payout.NonLifeProduction = nonLifeTransactions.total;
                        nonLifeData.Remove(nonLifeTransactions);
                    }
                    if (lifeTransactions != null)
                    {
                        payout.LifeProduction = lifeTransactions.total;
                        lifeData.Remove(lifeTransactions);
                    }

                }
                payoutList.Add(payout);
            }
            foreach (var item in previousLifeData)
            {
                var payout = new Payout { AgentCode = item.agentCode, AgentName = item.agentName, PreviousNonLifeProduction = item.total };
                if (item.total < _lifeQulifier)
                {
                    var nonLifeTransactions = nonLifeData.Find(m => m.agentCode == item.agentCode);
                    var lifeTransactions = lifeData.Find(m => m.agentCode == item.agentCode);
                    if (nonLifeTransactions != null)
                    {
                        payout.NonLifeProduction = nonLifeTransactions.total;
                        nonLifeData.Remove(nonLifeTransactions);
                    }
                    if (lifeTransactions != null)
                    {
                        payout.LifeProduction = lifeTransactions.total;
                        lifeData.Remove(lifeTransactions);
                    }
                }
                payoutList.Add(payout);
            } 
            #endregion
            //Every other person gets a bonus
            foreach (var item in nonLifeData)
            {
                var payout = new Payout 
                            { 
                                AgentCode = item.agentCode, 
                                AgentName = item.agentName,
                                NonLifeProduction=item.total,
                                LifeProduction=lifeData.Find(m=>m.agentCode==item.agentCode)==null?0:lifeData.Find(m=>m.agentCode==item.agentCode).total,
                            };
                var prevLife = previousLifeData.Find(m => m.agentCode == item.agentCode);
                var prevNonLife = previousNonLifeData.Find(m => m.agentCode == item.agentCode);
                
                if (prevLife != null)
                {
                    payout.PreviousLifeProduction = prevLife.total;
                    var nonLifeMultiplier=((decimal)(50.0/100)* payout.NonLifeProduction)+payout.NonLifeProduction;
                    var lifeMultiplier=payout.LifeProduction;
                    var multiplier=nonLifeMultiplier+lifeMultiplier;

                    payout.QuickStart=performanceBonusCategories.Find(m=>m.MinimumPremium<=multiplier && m.MaximumPremium>=multiplier).Bonus;
                }
                if (prevNonLife != null)
                {
                    payout.PreviousNonLifeProduction = prevNonLife.total;

                }
                payoutList.Add(payout);
            }
            return payoutList;
        }
        public List<Payout> GetFixedPayout(IQueryable<Agent> agents,DateTime productionDate, ref string msg)
        {
            try
            {
                //productionDate should be production end date
                //Pay Agents fixed rate for 3months
                var productionMonth = productionDate.Month;
                var productionYear = productionDate.Year;
                //var agents = new ServiceManager<Agent>().GetAll();
                
                //filter agents who have worked for over 3 months
                agents = agents.Where(m => m.StartDate <= productionDate && m.StartDate >= productionDate.AddMonths(-3));
                //agents = agents.FindAll(m => m.StartDate <= productionDate && m.StartDate >= productionDate.AddMonths(-3));
                //Get Salaries
                var fixedPay = new ServiceManager<Salary>().GetAll().ToList();
                if (!fixedPay.Any())
                {
                    msg = "Fixed pay was not found. Kindly setup fixed pay";
                    return null;
                }
                var paymentSchedule = new List<Payout>();
                foreach (var agent in agents)
                {
                    if(!agent.AgentType.Equals("Supervisor") || !agent.AgentType.Equals("Agent"))
                    {
                        agent.AgentType = CleanAgentType(agent.AgentType);
                    }
                    msg = agent.AgentType;
                    var payout = new Payout
                    {
                        AgentName = agent.AgentName,
                        AgentCode = agent.AgentCode,
                        FixedPay = fixedPay.Find(m => m.AgentType.Equals(agent.AgentType, StringComparison.InvariantCultureIgnoreCase)).Amount,
                        Savings =_savings, 
                    };
                    //agent.StartDate = DateTime.Now.AddDays(-1);
                    //prorate salary
                    if(agent.StartDate.Month==productionMonth && agent.StartDate.Year==productionYear)
                    {
                        int daysInMonth = DateTime.DaysInMonth(productionYear, productionMonth);
                        var dailyPay = payout.FixedPay / daysInMonth;
                        payout.FixedPay = payout.FixedPay/(daysInMonth - agent.StartDate.Day);
                        
                    }
                    paymentSchedule.Add(payout);
                }
                return paymentSchedule;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                JUtility.ErrorLog.LogApplicationError(ex);
                return null;
            }
            
        }

        private string CleanAgentType(string agentType)
        {
            if (agentType.Equals("FA", StringComparison.InvariantCultureIgnoreCase))
            {
                return "Supervisor";
            }
            else if(agentType.Equals("AGENCY", StringComparison.InvariantCultureIgnoreCase))
            {
                return "Agent";
            }
            else
            {
                return "Agent";
            }
        }
    }
}
