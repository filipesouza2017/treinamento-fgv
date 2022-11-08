using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDeTreinamento
{
    class ConnectionFactory
    {
        public static DateTime LastServiceRecovery { get; set; }
        public static TimeSpan ServiceRecoveryTime { get; set; }
        public static IOrganizationService Service { get; set; }

        public static IOrganizationService GetConnection()
        {
            ServiceRecoveryTime = new TimeSpan(0, 1, 0);

            if (Service == null)
            {
                GetCrmServiceClient();
            }
            else
            {
                TimeSpan timeDifference = DateTime.Now - LastServiceRecovery;

                if(timeDifference.TotalMinutes > ServiceRecoveryTime.TotalMinutes)
                {
                    Service = null;
                    GetCrmServiceClient();
                }
            }

            return Service;
        }

        private static void GetCrmServiceClient()
        {
            LastServiceRecovery = DateTime.Now;

            //CrmServiceClient.MaxConnectionTimeout = new TimeSpan(1, 0, 0);

            CrmServiceClient serviceClient = new CrmServiceClient(
                "AuthType=OAuth;" +
                "Username=admin@mdbfgv.onmicrosoft.com;" +
                "Password=P@ssw0rd;" +
                "Url=https://org95f33a51.crm2.dynamics.com/;" +
                "AppId=7d84cc19-4d89-4db5-8503-d54615eb9f91;" +
                "RedirectUri=app://58145B91-0C36-4500-8554-080854F2AC97;"
            );
            
            Service = serviceClient;
        }
    }
}
