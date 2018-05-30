using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AFTipsetWebService.Model
{
    [DataContract]
    public class UserResultAll
    {
        [DataMember(Order = 0)]
        public string User { get; set; }

        [DataMember(Order = 1)]
        public double MatchResult { get; set; }

        [DataMember(Order = 2)]
        public double TeamResult { get; set; }

        [DataMember(Order = 3)]
        public double ScorerResult { get; set; }

        [DataMember(Order = 4)]
        public double TotalResult { get; set; }
    }
}
