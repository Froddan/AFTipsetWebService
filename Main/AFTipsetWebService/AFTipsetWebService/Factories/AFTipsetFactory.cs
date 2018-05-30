using AFTipsetWebService.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFTipsetWebService.Factories
{
    public static class AFTipsetFactory
    {
        public static IAFTipsetDAL CreateDataAccess()
        {
            return new AFTipsetDAL();
        }
    }
}
