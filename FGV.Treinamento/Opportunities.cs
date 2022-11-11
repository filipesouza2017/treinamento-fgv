using FGV.Treinamento.Common;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FGV.Treinamento
{
    public class Opportunities : PluginImplement
    {
        public override void ExecutePlugin(IServiceProvider serviceProvider)
        {
            #region Exemplos com imagens
            //Entity preImage = (Entity)this.Context.PreEntityImages["PreImage"];
            //Entity preImage = (Entity)this.Context.PreEntityImages["PreImage"];
            //Entity postImage = (Entity)this.Context.PostEntityImages["PostImage"];

            //string messages = string.Empty;

            //if (preImage != null)
            //{
            //    if (preImage.Contains("name"))
            //        messages += $"Target: NAME: {preImage["name"].ToString()}";
            //}

            //throw new InvalidPluginExecutionException(messages);
            #endregion

            Entity opportunityPostImage = (Entity)this.Context.PostEntityImages["PostImage"];

            Entity account = GetAccount(opportunityPostImage, new string[] { "mib_valortotaldeoportunidades" });

            account["mib_valortotaldeoportunidades"] = new Money(
                ((Money)account["mib_valortotaldeoportunidades"]).Value + 
                ((Money)opportunityPostImage["totalamount"]).Value
            );

            this.Service.Update(account);

            if (this.Context.MessageName == PluginImplement.MessageNames.Create.ToString() || this.Context.MessageName == PluginImplement.MessageNames.Delete.ToString())
                CreateAndDelete();
        }

        private static string IncrementMessage(Entity opportunityTarget, string stage)
        {
            if (opportunityTarget.Contains("totalamount"))
                return $"{stage}: {((Money)opportunityTarget["totalamount"]).Value.ToString()}";

            return string.Empty;
        }

        private void CreateAndDelete()
        {
            Entity opportunity = this.Context.MessageName == PluginImplement.MessageNames.Create.ToString() ? (Entity)this.Context.InputParameters["Target"] : (Entity)this.Context.PreEntityImages["PreImage"];

            this.TracingService.Trace("Recuperou registro da oportunidade");

            if (opportunity.Contains("parentaccountid"))
            {
                Entity account = GetAccount(opportunity, new string[] { "mib_numerototaldeoportunidades" });

                int numeroTotalDeOportunidades = account.Contains("mib_numerototaldeoportunidades") ? (int)account["mib_numerototaldeoportunidades"] : 0;

                if (this.Context.MessageName == PluginImplement.MessageNames.Create.ToString())
                    numeroTotalDeOportunidades += 1;
                else
                    numeroTotalDeOportunidades -= 1;

                this.TracingService.Trace($"Número total {numeroTotalDeOportunidades}");

                account["mib_numerototaldeoportunidades"] = numeroTotalDeOportunidades;
                this.Service.Update(account);
            }
        }

        private Entity GetAccount(Entity opportunity, string[] columns)
        {
            EntityReference accountReference = (EntityReference)opportunity["parentaccountid"];

            Entity account = this.Service.Retrieve(
                accountReference.LogicalName,
                accountReference.Id,
                new ColumnSet(columns)
            );
            return account;
        }
    }
}
