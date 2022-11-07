using ConsoleDeTreinamento.Model;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDeTreinamento
{
    class Program
    {
        static void Main(string[] args)
        {
            IOrganizationService service = ConnectionFactory.GetConnection();

            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='account'>
                                    <attribute name='name' />
                                    <attribute name='primarycontactid' />
                                    <attribute name='accountid' />
                                    <order attribute='name' descending='false' />
                                    <link-entity name='contact' from='contactid' to='primarycontactid' link-type='inner' alias='contact'>
                                      <attribute name='telephone1' />
                                      <filter type='and'>
                                        <condition attribute='telephone1' operator='not-null' />
                                      </filter>
                                    </link-entity>
                                  </entity>
                                </fetch>";

            EntityCollection accounts = service.RetrieveMultiple(
                new FetchExpression(fetchXml));

            foreach(Entity account in accounts.Entities)
            {
                //AliasedValue
                Console.WriteLine(account["contact.telephone1"].ToString());
            }

            Console.ReadKey();
        }

        
    }
}
