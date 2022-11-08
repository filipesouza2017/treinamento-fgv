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

            #region Exemplo recuperando Id no laço
            //List<Guid> accountIds = new List<Guid>();

            //foreach (Entity account in accounts.Entities)
            //{
            //    accountIds.Add(account.Id);
            //}
            #endregion

            IOrganizationService service = ConnectionFactory.GetConnection();
            EntityCollection accounts = QueryAccounts(service);

            List<ConditionExpression> conditions = new List<ConditionExpression>();
            AddConditions(accounts, conditions);
            EntityCollection allContacts = QueryContacts(service, conditions);

            foreach (Entity account in accounts.Entities)
            {
                Console.WriteLine(account["name"].ToString());

                var contactByAccount = (from contact in allContacts.Entities
                                        where ((EntityReference)contact["parentcustomerid"]).Id == account.Id
                                        select contact).ToList();

                foreach (var contact in contactByAccount)
                {
                    Console.WriteLine(contact["fullname"].ToString());
                }

                Console.WriteLine("----------------");
            }

            Console.ReadKey();
        }

        private static EntityCollection QueryContacts(IOrganizationService service, List<ConditionExpression> conditions)
        {
            QueryExpression queryContacts = new QueryExpression("contact");
            queryContacts.ColumnSet.AddColumns("fullname", "parentcustomerid");

            FilterExpression filterByParent = new FilterExpression();
            filterByParent.FilterOperator = LogicalOperator.Or;
            filterByParent.Conditions.AddRange(conditions);
            queryContacts.Criteria.AddFilter(filterByParent);

            EntityCollection allContacts = service.RetrieveMultiple(queryContacts);
            return allContacts;
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
