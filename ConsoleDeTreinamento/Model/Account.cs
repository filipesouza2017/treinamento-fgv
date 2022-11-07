using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDeTreinamento.Model
{
    public class Account
    {
        public Guid Create(IOrganizationService service, int totalDeOportunidades, string cnpj, string nomeDaConta, OptionSetValue nivelDoCliente, Money valorTotalDeOportunidades, EntityReference contatoPrimario, Guid customId)
        {
            Entity conta = MakeEntity(totalDeOportunidades, cnpj, nomeDaConta, nivelDoCliente, valorTotalDeOportunidades, contatoPrimario, customId);
            Guid contaId = service.Create(conta);
            return contaId;
        }

        public Entity MakeEntity(int totalDeOportunidades, string cnpj, string nomeDaConta, OptionSetValue nivelDoCliente, Money valorTotalDeOportunidades, EntityReference contatoPrimario, Guid customId)
        {
            Entity conta = new Entity("account", customId);
            conta["mib_cnpj"] = cnpj;
            conta["name"] = nomeDaConta;
            conta["mib_numerototaldeoportunidades"] = totalDeOportunidades;
            conta["mib_niveldaconta"] = nivelDoCliente;
            conta["mib_valortotaldeoportunidades"] = valorTotalDeOportunidades;
            conta["primarycontactid"] = contatoPrimario;
            return conta;
        }

        public void Update(IOrganizationService service, EntityReference contatoPrimario, Guid id)
        {
            Entity conta = new Entity("account", id);
            //account.Id = new Guid("70a9e8d8-3e5c-ed11-9562-000d3a888b78");
            //conta["telephone1"] = "+55 (11) 4361-8535";
            //conta["mib_cnpj"] = cnpj;
            //conta["name"] = nomeDaConta;
            //conta["mib_numerototaldeoportunidades"] = totalDeOportunidades;
            conta["primarycontactid"] = contatoPrimario;
            service.Update(conta);
        }

        public void Delete(IOrganizationService service, Guid id)
        {
            service.Delete("account", id);
        }

        public void ExecuteLesson2(IOrganizationService service)
        {
            int totalDeOportunidades = 2;
            string cnpj = "13.1254.214/0001-65";
            string nomeDaConta = "Hello World! Atualizado";

            //Tipos Picklist
            OptionSetValue nivelDoCliente = new OptionSetValue(863260002);
            Money valorTotalDeOportunidades = new Money(
                new decimal(1.300)
            );

            EntityReference contatoPrimario = new EntityReference(
                "contact",
                new Guid("15a17064-1ae7-e611-80f4-e0071b661f01")
            );

            MakeExecuteMultipleRequest(service, totalDeOportunidades, cnpj, nomeDaConta, nivelDoCliente, valorTotalDeOportunidades, contatoPrimario);
        }

        public void MakeExecuteMultipleRequest(IOrganizationService service, int totalDeOportunidades, string cnpj, string nomeDaConta, OptionSetValue nivelDoCliente, Money valorTotalDeOportunidades, EntityReference contatoPrimario)
        {
            ExecuteMultipleRequest makeRequest = new ExecuteMultipleRequest()
            {
                Settings = new ExecuteMultipleSettings()
                {
                    ContinueOnError = false,
                    ReturnResponses = true
                },
                Requests = new OrganizationRequestCollection()
            };

            Account contaModel = new Account();

            for (int i = 0; i < 100; i++)
            {
                Guid id = Guid.NewGuid();

                if (i == 10)
                    id = new Guid("72099e4e-31fc-45cf-a6de-1e23b88a0de0");
                else
                    if (i == 11)
                    id = new Guid("d4e7a6f3-80f1-45f1-9fcb-3cfbb5c8f3bd");

                Entity novaConta = contaModel.MakeEntity(
                    totalDeOportunidades,
                    cnpj,
                    nomeDaConta + (i * 2),
                    nivelDoCliente,
                    valorTotalDeOportunidades,
                    contatoPrimario,
                    id
                );

                UpsertRequest upsertRequest = new UpsertRequest()
                {
                    Target = novaConta
                };
                makeRequest.Requests.Add(upsertRequest);
            }

            ExecuteMultipleResponse executeMultipleResponse = (ExecuteMultipleResponse)service.Execute(makeRequest);

            foreach (var responseItem in executeMultipleResponse.Responses)
            {
                Console.WriteLine(responseItem.RequestIndex);

                //A criação realmente aconteceu
                if (responseItem.Response != null)
                    Console.WriteLine(responseItem.Response);

                if (responseItem.Fault != null)
                    Console.WriteLine(responseItem.Fault);
            }
        }

        public void MakeCreateRequest(ExecuteMultipleRequest makeRequest, Entity novaConta)
        {
            CreateRequest createRequest = new CreateRequest()
            {
                Target = novaConta
            };
            makeRequest.Requests.Add(createRequest);
        }

        private static Entity Retrieve(IOrganizationService service)
        {
            Guid accountId = new Guid("02822722-3d9c-473f-b322-0434ea253951");

            Entity account =
                service.Retrieve(
                    "account",
                    accountId,
                    new ColumnSet("name", "mib_cnpj", "mib_numerototaldeoportunidades", "primarycontactid", "mib_valortotaldeoportunidades", "mib_niveldaconta")
                );
            return account;
        }

        public void GetPicklistMetadata(IOrganizationService service, Entity account)
        {
            RetrieveAttributeRequest attributeRequest = new RetrieveAttributeRequest();
            attributeRequest.EntityLogicalName = "account";
            attributeRequest.LogicalName = "mib_niveldaconta";
            RetrieveAttributeResponse attributeResponse = (RetrieveAttributeResponse)service.Execute(attributeRequest);
            PicklistAttributeMetadata mibNivelDaContaMeta = (PicklistAttributeMetadata)attributeResponse.AttributeMetadata;

            foreach (OptionMetadata option in mibNivelDaContaMeta.OptionSet.Options)
            {
                if (option.Value == ((OptionSetValue)account["mib_niveldaconta"]).Value)
                {
                    Console.WriteLine(option.Label.UserLocalizedLabel.Label.ToString());
                }
            }
        }

        public void RetrieveByFetch(IOrganizationService service)
        {
            string fetchXML = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='account'>
                                    <attribute name='name' />
                                    <attribute name='primarycontactid' />
                                    <attribute name='mib_cnpj' />
                                    <attribute name='telephone1' />
                                    <attribute name='accountid' />
                                    <order attribute='name' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='name' operator='like' value='H%' />
                                    </filter>
                                  </entity>
                                </fetch>";

            EntityCollection contas = service.RetrieveMultiple
                (new FetchExpression(fetchXML));

            foreach (Entity account in contas.Entities)
            {
                Console.WriteLine(
                    account["name"].ToString()
                );
                Console.WriteLine(
                    account["mib_cnpj"].ToString()
                );

                if (account.Contains("mib_numerototaldeoportunidades"))
                {
                    Console.WriteLine(
                        account["mib_numerototaldeoportunidades"].ToString()
                    );
                }

                Console.WriteLine(
                    account.Contains("primarycontactid") ? ((EntityReference)account["primarycontactid"]).Id + " - " + ((EntityReference)account["primarycontactid"]).Name : string.Empty
                );

                //Console.WriteLine(
                //    ((Money)account["mib_valortotaldeoportunidades"]).Value.ToString()
                //);
                //Console.WriteLine(
                //    ((OptionSetValue)account["mib_niveldaconta"]).Value.ToString()
                //);
            }
        }
    }
}
