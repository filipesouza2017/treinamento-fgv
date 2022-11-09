using ConsoleDeTreinamento.Model;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
            
            Console.ReadKey();
        }


        private static void QueryExpressionContacts(IOrganizationService service)
        {
            EntityCollection allContacts = QueryContacts(service);

            foreach (Entity contact in allContacts.Entities)
            {
                Console.WriteLine("CONTATO:");
                Console.WriteLine(contact["fullname"].ToString());
                Console.WriteLine(contact["telephone1"].ToString());

                Console.WriteLine("TELEFONE DA CONTA");
                Console.WriteLine(((AliasedValue)contact["account.telephone1"]).Value.ToString());
            }
        }

        private static EntityCollection QueryContacts(IOrganizationService service)
        {
            QueryExpression queryContacts = new QueryExpression("contact");
            queryContacts.ColumnSet.AddColumns("fullname", "telephone1");

            queryContacts.AddLink("account", "accountid", "parentcustomerid", JoinOperator.Inner);
            queryContacts.LinkEntities.FirstOrDefault().EntityAlias = "account";
            queryContacts.LinkEntities.FirstOrDefault().LinkCriteria.AddCondition("telephone1", ConditionOperator.NotNull);
            queryContacts.LinkEntities.FirstOrDefault().Columns.AddColumns("telephone1");

            return service.RetrieveMultiple(queryContacts);
        }

        private static void AddConditions(EntityCollection accounts, List<ConditionExpression> conditions)
        {
            List<Guid> accountIdsLinq = (from account in accounts.Entities
                                         select account.Id).Distinct().ToList();

            foreach (Guid accountId in accountIdsLinq)
            {
                conditions.Add(
                    new ConditionExpression("parentcustomerid", ConditionOperator.Equal, accountId)
                );
            }
        }

        private static EntityCollection QueryAccounts(IOrganizationService service)
        {
            QueryExpression queryAccounts = new QueryExpression("account");
            //queryAccounts.ColumnSet.AllColumns = true;
            queryAccounts.ColumnSet.AddColumns("name", "telephone1", "primarycontactid");
            queryAccounts.Criteria.AddCondition("name", ConditionOperator.Like, "A%");
            queryAccounts.Criteria.AddCondition("telephone1", ConditionOperator.NotNull);
            EntityCollection accounts = service.RetrieveMultiple(queryAccounts);
            return accounts;
        }
    }
}
