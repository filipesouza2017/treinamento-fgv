using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDeTreinamento.Model
{
    public class Contact
    {
        public string TableName = "contact";

        private EntityCollection QueryContacts(IOrganizationService service, List<ConditionExpression> conditions)
        {
            QueryExpression queryContacts = new QueryExpression(this.TableName);
            queryContacts.ColumnSet.AddColumns("fullname", "parentcustomerid");

            FilterExpression filterByParent = new FilterExpression();
            filterByParent.FilterOperator = LogicalOperator.Or;
            filterByParent.Conditions.AddRange(conditions);
            queryContacts.Criteria.AddFilter(filterByParent);

            EntityCollection allContacts = service.RetrieveMultiple(queryContacts);
            return allContacts;
        }

        private static void WriteContactsByAccountLinq(EntityCollection allContacts)
        {
            EntityCollection accounts = new EntityCollection();

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
        }


        private static void QueryContactsBySql()
        {
            string connectionString =
                            "Server=orgf5c130e4.crm2.dynamics.com,5558;" +
                            "Authentication=Active Directory Password;" +
                            "Database=orgf5c130e4;" +
                            $"User Id={ConnectionFactory.UserName};" +
                            $"Password={ConnectionFactory.Password};";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                using (SqlCommand queryContacts = new SqlCommand())
                {
                    queryContacts.Connection = sqlConnection;
                    queryContacts.CommandText = @"SELECT name, primarycontactid, account.accountid, contact.fullname, primarycontactidname
                                                  FROM account
                                                  INNER JOIN contact
                                                          ON contact.contactid = account.primarycontactid
                                                  WHERE account.telephone1 IS NOT NULL";

                    using (SqlDataReader dataReader = queryContacts.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Console.WriteLine(dataReader["name"]);
                            Console.WriteLine(dataReader["primarycontactid"]);
                            Console.WriteLine(dataReader["primarycontactidname"]);
                            Console.WriteLine(dataReader["fullname"]);
                            Console.WriteLine(dataReader["accountid"]);
                            Console.WriteLine("---------------");
                        }
                    }
                }
            }
        }
    }
}
