using AFTipsetWebService.Factories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Web;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace AFTipsetServiceHost
{
    public partial class AFTipsetWinService : ServiceBase
    {
        private WebServiceHost m_webhost = new WebServiceHost();

        public AFTipsetWinService()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if DEBUG
            try
            {
                Console.WriteLine("Starting ÅF Tipset Web Service");
                var impl = new AFTipsetWebService.AFTipsetService(AFTipsetFactory.CreateDataAccess());
                WebServiceHost webhost = new WebServiceHost(impl);
                webhost.Open();
                Console.WriteLine("Listening on endpoints: " + webhost.BaseAddresses[0].ToString());
                Console.WriteLine("Press enter to exit");
                Console.ReadLine();

                webhost.Close();
            }
            catch (Exception e)
            {
                var prevColor = Console.ForegroundColor;
                try
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("An error occured when starting the Web Service: " + e.Message);
                    Console.WriteLine(e.StackTrace);
                }
                finally
                {
                    Console.ForegroundColor = prevColor;
                }
                Console.WriteLine("Press enter to exit");
                Console.ReadLine();
            }
#else
            ServiceBase.Run(new AFTipsetWinService());
#endif
        }

        protected override void OnStart(string[] args)
        {
            m_webhost = new WebServiceHost(typeof(AFTipsetWebService.AFTipsetService));
            m_webhost.Open();
        }

        protected override void OnStop()
        {
            if(m_webhost != null)
            {
                m_webhost.Close();
            }
        }
    }
}
