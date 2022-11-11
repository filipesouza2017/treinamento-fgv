using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGV.Treinamento.Common
{
    public abstract class PluginImplement : IPlugin
    {
        public IPluginExecutionContext Context { get; set; }
        public ITracingService TracingService { get; set; }
        public IOrganizationServiceFactory ServiceFactory { get; set; }
        public IOrganizationService Service { get; set; }

        public enum StageNumbers
        {
            PreValidation = 10,
            PreOperation = 20,
            MainOperation = 30,
            PostOperation = 40
        }

        public enum MessageNames
        {
            Create,
            Update,
            Delete
        }

        public void Execute(IServiceProvider serviceProvider)
        {
            Context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            TracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            ServiceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            Service = ServiceFactory.CreateOrganizationService(Context.UserId);

            try
            {
                ExecutePlugin(serviceProvider);
            }
            catch(Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.ToString());
            }
        }

        public abstract void ExecutePlugin(IServiceProvider serviceProvider);
    }
}
