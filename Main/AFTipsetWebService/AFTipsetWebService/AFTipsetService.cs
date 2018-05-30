using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using AFTipsetWebService.Model;
using AFTipsetWebService.DataAccess;

namespace AFTipsetWebService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class AFTipsetService : IAFTipsetService
    {
        private readonly IAFTipsetDAL m_dataAccess;

        public AFTipsetService(IAFTipsetDAL dataAccess)
        {
            this.m_dataAccess = dataAccess;
        }

        public List<UserResultAll> GetUserResultsForAll()
        {
            return m_dataAccess.GetUserResultsForAll();
        }

        public List<UserResult> GetUserResultsForMatches()
        {
            return m_dataAccess.GetUserResultsForMatches();
        }

        public List<UserResult> GetUserResultsForScorers()
        {
            return m_dataAccess.GetUserResultsForScorers();
        }

        public List<UserResult> GetUserResultsForTeams()
        {
            return m_dataAccess.GetUserResultsForTeams();
        }
    }
}
