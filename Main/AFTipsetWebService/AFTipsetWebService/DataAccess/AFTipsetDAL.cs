using AFTipsetWebService.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFTipsetWebService.DataAccess
{
    class AFTipsetDAL : IAFTipsetDAL
    {
        private MySqlConnection GetConnection()
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["AFTipset"].ConnectionString;

            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        public List<UserResult> GetUserResultsForMatches()
        {
            const string query = @"Select U.UserSign,
                                       Round(Sum(CASE M.Result1X2
                                                 WHEN '1' Then M.OddsResult1
                                                 WHEN 'X' Then M.OddsResultX
                                                 WHEN '2' Then M.OddsResult2
                                                 ELSE 0 END), 2) MatchResult
                                   From TP_USERS AS U
                                   Inner Join TP_USER_TIP_MATCHES AS UM on UM.UserID = U.UserID
                                  Inner Join TP_MATCHES AS M on M.CompID = UM.CompID and M.MatchNo = UM.MatchNo and M.Result1X2 = UM.MatchTip
                                Where UM.CompID = (Select max(CompID) from TP_COMPETITIONS)
                                Group By U.UserSign
                                Order By 2 desc";

            List<UserResult> results = new List<UserResult>();
            using (var connection = GetConnection())
            {
                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UserResult result = new UserResult();
                        result.User = reader.GetString("UserSign");
                        result.Result = reader.GetDouble("MatchResult");
                        results.Add(result);
                    }
                }
            }

            return results;
        }

        public List<UserResult> GetUserResultsForTeams()
        {
            const string query = @"Select U.UserSign,
       Round(Sum(CASE T.StageQualify1 WHEN UT.StageQualify1 THEN T.StageOdds1 ELSE 0 END +
                                                       CASE T.StageQualify2 WHEN UT.StageQualify2 THEN T.StageOdds2 ELSE 0 END +
                                                       CASE T.StageQualify3 WHEN UT.StageQualify3 THEN T.StageOdds3 ELSE 0 END +
                                                       CASE T.StageQualify4 WHEN UT.StageQualify4 THEN T.StageOdds4 ELSE 0 END +
                                                       CASE T.StageQualify5 WHEN UT.StageQualify5 THEN T.StageOdds5 ELSE 0 END),2) TeamResult
  From TP_USERS AS U
  Inner Join TP_USER_TIP_TEAMS AS UT on UT.UserID = U.UserID 
  Inner Join TP_TEAMS AS T on T.CompID = UT.CompID and T.TeamNo = UT.TeamNo
Where UT.CompID = (Select max(CompID) from TP_COMPETITIONS)
Group By U.UserSign
 Order By 2 desc ";

            List<UserResult> results = new List<UserResult>();
            using (var connection = GetConnection())
            {
                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UserResult result = new UserResult();
                        result.User = reader.GetString("UserSign");
                        result.Result = reader.GetDouble("TeamResult");
                        results.Add(result);
                    }
                }
            }

            return results;
        }

        public List<UserResult> GetUserResultsForScorers()
        {
            const string query = @"Select U.UserSign,
                                       Round(Sum(S.ScorerGoals),2) ScorerResult
                                  From TP_USERS AS U
                                  Inner Join TP_USER_TIP_SCORERS AS US on US.UserID = U.UserID 
                                  Inner Join TP_SCORERS AS S on S.CompID = US.CompID and S.ScorerID = US.ScorerID
                                Where US.CompID = (Select max(CompID) from TP_COMPETITIONS)
                                Group By U.UserSign
                                 Order By 2 desc ";

            List<UserResult> results = new List<UserResult>();
            using (var connection = GetConnection())
            {
                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UserResult result = new UserResult();
                        result.User = reader.GetString("UserSign");
                        result.Result = reader.GetDouble("ScorerResult");
                        results.Add(result);
                    }
                }
            }

            return results;
        }

        public List<UserResultAll> GetUserResultsForAll()
        {
            const string query = @"Select X.UserSign,
                                       Round(Sum(X.MatchResult),2) MatchResult,
                                       Round(Sum(X.TeamResult),2) TeamResult,
                                                                             Round(Sum(X.ScorerResult),2) ScorerResult,
                                       Round(Sum(X.MatchResult + X.TeamResult + X.ScorerResult),2) TotalResult
                                From (
                                Select U.UserSign,
                                       CASE M.Result1X2
                                       WHEN '1' Then M.OddsResult1
                                       WHEN 'X' Then M.OddsResultX
                                       WHEN '2' Then M.OddsResult2
                                       ELSE 0 END MatchResult,
                                       0 TeamResult,
                                                                            0 ScorerResult
                                  From TP_USERS AS U
                                  Inner Join TP_USER_TIP_MATCHES AS UM on UM.UserID = U.UserID 
                                  Inner Join TP_MATCHES AS M on M.CompID = UM.CompID and M.MatchNo = UM.MatchNo and M.Result1X2 = UM.MatchTip
                                Where UM.CompID = (Select max(CompID) from TP_COMPETITIONS)
                                Union All
                                Select U.UserSign,
                                                                             0 MatchResult,
                                       CASE T.StageQualify1 WHEN UT.StageQualify1 THEN T.StageOdds1 ELSE 0 END +
                                       CASE T.StageQualify2 WHEN UT.StageQualify2 THEN T.StageOdds2 ELSE 0 END +
                                       CASE T.StageQualify3 WHEN UT.StageQualify3 THEN T.StageOdds3 ELSE 0 END +
                                       CASE T.StageQualify4 WHEN UT.StageQualify4 THEN T.StageOdds4 ELSE 0 END +
                                       CASE T.StageQualify5 WHEN UT.StageQualify5 THEN T.StageOdds5 ELSE 0 END TeamResult,
                                                                            0 ScorerResult
                                  From TP_USERS AS U
                                  Inner Join TP_USER_TIP_TEAMS AS UT on UT.UserID = U.UserID 
                                  Inner Join TP_TEAMS AS T on T.CompID = UT.CompID and T.TeamNo = UT.TeamNo
                                Where UT.CompID = (Select max(CompID) from TP_COMPETITIONS)
                                Union All
                                Select U.UserSign,
                                                                             0 MatchResult,
                                       0 TeamResult,
                                       S.ScorerGoals ScorerResult
                                  From TP_USERS AS U
                                  Inner Join TP_USER_TIP_SCORERS AS US on US.UserID = U.UserID 
                                  Inner Join TP_SCORERS AS S on S.CompID = US.CompID and S.ScorerID = US.ScorerID
                                Where US.CompID = (Select max(CompID) from TP_COMPETITIONS)
                                ) X
                                Group By X.UserSign
                                 Order By 5 desc";

            List<UserResultAll> results = new List<UserResultAll>();
            using (var connection = GetConnection())
            {
                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UserResultAll result = new UserResultAll();
                        result.User = reader.GetString("UserSign");
                        result.MatchResult = reader.GetDouble("MatchResult");
                        result.TeamResult = reader.GetDouble("TeamResult");
                        result.ScorerResult = reader.GetDouble("ScorerResult");
                        result.TotalResult = reader.GetDouble("TotalResult");
                        results.Add(result);
                    }
                }
            }

            return results;
        }
    }
}
