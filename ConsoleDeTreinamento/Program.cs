using ConsoleDeTreinamento.Model;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
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

        private static void MakeCreateRequest(ExecuteMultipleRequest makeRequest, Entity novaConta)
        {
            CreateRequest createRequest = new CreateRequest()
            {
                Target = novaConta
            };
            makeRequest.Requests.Add(createRequest);
        }
    }
}
