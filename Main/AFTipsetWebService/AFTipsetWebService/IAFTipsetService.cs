using AFTipsetWebService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace AFTipsetWebService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IAFTipsetService
    {
        [OperationContract]
        [WebGet(UriTemplate = "api/aftipset/result/matches", ResponseFormat = WebMessageFormat.Json)]
        List<UserResult> GetUserResultsForMatches();

        [OperationContract]
        [WebGet(UriTemplate = "api/aftipset/result/teams", ResponseFormat = WebMessageFormat.Json)]
        List<UserResult> GetUserResultsForTeams();

        [OperationContract]
        [WebGet(UriTemplate = "api/aftipset/result/scorers", ResponseFormat = WebMessageFormat.Json)]
        List<UserResult> GetUserResultsForScorers();

        [OperationContract]
        [WebGet(UriTemplate = "api/aftipset/result", ResponseFormat = WebMessageFormat.Json)]
        List<UserResultAll> GetUserResultsForAll();

        // TODO: Add your service operations here
    }

   
}
