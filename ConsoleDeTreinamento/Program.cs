using Microsoft.Xrm.Sdk;
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

            int totalDeOportunidades = 1;
            string cnpj = "12.1254.214/0001-55";
            string nomeDaConta = "Hello World! 2";

            //Tipos Picklist
            OptionSetValue nivelDoCliente = new OptionSetValue(863260002);
            Money valorTotalDeOportunidades = new Money(
                new decimal(1.300)
            );

            EntityReference contatoPrimario = new EntityReference(
                "contact",
                new Guid("7469fd95-c0bd-4236-90bf-1d1100291df5")
            );

            Guid customId = Guid.NewGuid();

            Console.WriteLine($"CUSTOMID: {customId}");

            Entity conta = new Entity("account", customId);
            conta["mib_cnpj"] = cnpj;
            conta["name"] = nomeDaConta;
            conta["mib_numerototaldeoportunidades"] = totalDeOportunidades;
            conta["mib_niveldaconta"] = nivelDoCliente;
            conta["mib_valortotaldeoportunidades"] = valorTotalDeOportunidades;
            conta["primarycontactid"] = contatoPrimario;
            Guid contaId = service.Create(conta);
            Console.WriteLine(contaId);
            Console.ReadKey();
        }
    }
}
