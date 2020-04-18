using DataFactory.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataFactory.Services
{
    public class TransHelper
    {
        private string _tqPortalConnection=ConfigurationManager.AppSettings["tqPortalConnection"];
        public List<Agent> GetAgents()
        {
            try
            {
                var agents = new List<Agent>();
                // Check AgentDBTrans
                using (SqlConnection connection = new SqlConnection(_tqPortalConnection))
                {
                    SqlCommand command = new SqlCommand("SELECT agent_code_new,agent_name,agent_type FROM [AgentDBTrans].[dbo].[com_agent] where status='Active' and sbuid in (select id from sbuCoverageAreas where coverage_area_name like'%agency%')", connection);
                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var agent = new Agent
                            {
                                AgentCode= reader[0].ToString(),
                                AgentName= reader[1].ToString().ToUpper(),
                                AgentType = reader[2].ToString().ToUpper(),
                                StartDate =DateTime.Now.AddMonths(-6),
                            };
                            agents.Add(agent);
                        }
                    }
                    connection.Close();
                }
                return agents;
            }
            catch (Exception ex)
            {
                JUtility.ErrorLog.LogApplicationError(ex);
                return null;
            }
            
        }
    }
}
