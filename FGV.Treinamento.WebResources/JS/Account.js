if (typeof (FGV) == "undefined") { FGV = {} }
if (typeof (FGV.Account) == "undefined") { FGV.Account = {} }

FGV.Account = {
    OnLoad: function (executionContext) {
        var id = Xrm.Page.data.entity.getId();

        Xrm.WebApi.online.retrieveMultipleRecords("systemuser", "?$select=businessunitid&$filter=systemuserid eq " + id).then(
            function success(results) {
                if (results.entities.length > 0) {
                    if (results.entities[0]["businessunitid"] == "") {

                    }
                }
            },
            function (error) {

            }
        );

    }
}