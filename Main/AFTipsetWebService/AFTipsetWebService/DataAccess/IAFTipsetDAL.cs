using System.Collections.Generic;
using AFTipsetWebService.Model;

namespace AFTipsetWebService.DataAccess
{
    public interface IAFTipsetDAL
    {
        List<UserResultAll> GetUserResultsForAll();
        List<UserResult> GetUserResultsForMatches();
        List<UserResult> GetUserResultsForScorers();
        List<UserResult> GetUserResultsForTeams();
    }
}