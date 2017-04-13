(function(){
    app.controller("InvoicesCtrl", ['$scope', 'DataService',  function($scope, DataService) {
        $scope.showInvoice = false;
        getShippers('');
        getAgents('');
        getCustomers('');
        ListInvoices();

        //READ AND EDIT INVOICES
        $scope.edit = function(currentInvoice){
            $scope.invoice = currentInvoice;
            $scope.showInvoice = true;
        };

        //UPDATE INVOICE
        $scope.save = function(){
            if($scope.invoice.id == 0)
                DataService.insert("invoices", $scope.invoice, function(data){ ListInvoices();} );
            else
                DataService.update("invoices", $scope.invoice.id, $scope.invoice, function(data){ListInvoices();});
        };
        //CREATE NEW INVOICE
        $scope.new = function(){
            $scope.invoice = {
                id: 0,
                invoiceNo: "",
                date: new Date(),
                shippedOn: new Date(),
                status: 0,
                subTotal: 0,
                vat: 0,
                vatAmount: 0,
                total: 0,
                shipperId: 0,
                agentId: 0,
                customers: [],
                shipping: 0,
                items: []
            };
            document.getElementById('custsel').style.visibility = 'hidden';
            $scope.showInvoices = true;
        };

        
        
        //DELETE INVOICE
        $scope.delete = function (invoice) {
            DataService.delete("invoices", invoice.id, function (data) {
                ListInvoices();
            });
            $scope.showInvoices = false;
        }; 
        //LIST ALL INVOICES
        function ListInvoices(){
            DataService.list("invoices", function(data){ $scope.invoices = data});
        };

        $scope.statuses = [
            { "-1": "Canceled" },
            { "0": "OrderCreated" },
            { "1": "InvoiceCreated" },
            { "2": "InvoiceSent" },
            { "3": "InvoicePaid" },
            { "4": "InvoiceOnHold" },
            { "5": "InvoiceReady" },
            { "6": "InvoiceShipped" }
        ];

        $scope.getKey = function (status) {
            return Object.keys(status)[0];
        }

        //LIST/GET ALL SHIPPERS
        function getShippers(name){
            DataService.list("shippers/" + name, function(data){ $scope.shippers = data});
        };

        //LIST/GET ALL AGENTS
        function getAgents(name){
            DataService.list("agents", function(data){ $scope.agents = data});
        };
        
        //LIST/GET ALL CUSTOMERS
        function getCustomers(name){
            DataService.list("customers/" + name, function(data){ $scope.customers = data});
        };

        //ARROW DOWN EVENT SO IT COULD FOCUS ON DROPDOWN
        $scope.textUp = function(keyEvent){
                if(keyEvent.key == "ArrowDown") document.getElementById('custsel').focus();
            };

        $scope.customerSelected = function(keyEvent){
                if(keyEvent.key == "Enter") {
                    for(var i=0; i<$scope.customers.length; i++){
                        if($scope.customers[i].id === $scope.invoice.customerId){
                            $scope.invoice.customer = $scope.customers[i].name;
                            document.getElementById('custsel').style.visibility = 'hidden';
                            break;
                        }
                    }
                }
            };

        //AUTOCOMPLETE in BOX
        $scope.autocomplete = function(autoStr){
                if (autoStr.length >= 3){ //reaguje samo kada ima 3 ili više slova uneseno
                    getCustomers(autoStr);
                    document.getElementById('custsel').style.visibility = 'visible';//Otkrivamo combobox
                    document.getElementById('custsel').size = 8;
                }
                else {
                    document.getElementById('custsel').style.visibility = 'hidden';
                }
            };
    }]);

}());