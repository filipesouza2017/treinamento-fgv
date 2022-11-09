using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGV.Treinamento
{
    public class Opportunities : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            Entity opportunity = (Entity)context.InputParameters["Target"];

            tracingService.Trace("Recuperou registro da oportunidade");

            if (opportunity.Contains("parentaccountid"))
            {
                EntityReference accountReference = (EntityReference)opportunity["parentaccountid"];

                Entity account = service.Retrieve(
                    accountReference.LogicalName,
                    accountReference.Id,
                    new ColumnSet("mib_numerototaldeoportunidades")
                );

                int numeroTotalDeOportunidades = account.Contains("mib_numerototaldeoportunidades") ? (int)account["mib_numerototaldeoportunidades"] : 0;

                numeroTotalDeOportunidades += 1;

                tracingService.Trace($"Número total {numeroTotalDeOportunidades}");

                account["mib_numerototaldeoportunidades"] = numeroTotalDeOportunidades;
                service.Update(account);
            }
        }
    }
}
