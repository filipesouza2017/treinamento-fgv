using Microsoft.Xrm.Sdk;
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
    }
}
