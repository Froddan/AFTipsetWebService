using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AFTipsetWebService.Model
{
    [DataContract]
    public class UserResult
    {
        [DataMember(Order = 0)]
        public string User { get; set; }

        [DataMember(Order = 1)]
        public double Result { get; set; }
    }
}
