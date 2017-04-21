(function() {
    app.controller("InvoicesCtrl", ['$scope', 'DataService', '$http', function($scope, DataService, $http) {
        $scope.showInvoice = false;
        getShippers('');
        getAgents('');
        ListInvoices();

        $scope.selectedCustomer = { id: 0, name: "" };


        //READ AND EDIT INVOICES
        // $scope.edit = function(currentInvoice) {
        //     $scope.invoice = currentInvoice;
        //     $scope.showInvoice = true;
        // };

        $scope.edit = function (invoice) {
            if (invoice.id == 0) {
                $scope.selectedCustomer = { id: 0, name: '' };
            } else {
                $scope.selectedCustomer = { id: invoice.customerId, name: invoice.customer };
            }
            $scope.invoice = invoice;
        };

        //UPDATE INVOICE
        $scope.save = function() {
            console.log($scope.selectedCustomer);
            $scope.invoice.customerId = $scope.selectedCustomer.id;
            console.log($scope.invoice);
            if ($scope.invoice.id == 0)
                DataService.insert("invoices", $scope.invoice, function(data) { ListInvoices(); });
            else
                DataService.update("invoices", $scope.invoice.id, $scope.invoice, function(data) { ListInvoices(); });
        };

        //CREATE NEW INVOICE
        var dbase = new Date();
        var invGenNum = dbase.valueOf(); //Generate invoice number
        var currentDate = new Date();
        var dateShipped = new Date(new Date(currentDate).setDate(currentDate.getDate() + 5)); // Set default shipping date to current + 5 days.
        $scope.new = function() {
            $scope.invoice = {
                id: 0
            };
            $scope.showInvoices = true;

                // $scope.total = function() {
                //     var total = 0;
                //     angular.forEach($scope.invoice.items, function(item) {
                //         total += item.quantity * item.price;
                //     })
                //     return total;
                // }

        };
        //END OF ITEM SECTION IN MODAL


        //TYPEAHEAD PRODUCTS AND CUSTOMERS
        var _selected;
        $scope.selected = (undefined);

        $scope.getCustomers = function(name) {
            return $http.get('http://localhost:9000/api/customers/' + name).then(function(response) {
                return response.data;
            });
        };

        $scope.getProducts = function(name) {
            return $http.get('http://localhost:9000/api/products/' + name).then(function(response) {
                return response.data;
            });
        };

        $scope.ngModelOptionsSelected = function(value) {
            if (arguments.length) {
                _selected = value;
            } else {
                return _selected;
            }
        };

        $scope.modelOptions = {
            debounce: {
                default: 500,
                blur: 250
            },
            getterSetter: true
        };

        //DELETE INVOICE
        $scope.delete = function(invoice) {
            DataService.delete("invoices", invoice.id, function(data) {
                ListInvoices();
            });
            $scope.showInvoices = false;
        };
        //LIST ALL INVOICES
        function ListInvoices() {
            DataService.list("invoices", function(data) { $scope.invoices = data });
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

        // Ovaj dio sluzi za view onih sarenih buttona na invoice tabeli
        $scope.states = BillingConfig.stat;
        //--------------------------------------------------------------

        $scope.getKey = function(status) {
            return Object.keys(status)[0];
        }


        //GET AND SAVE ITEMS
        $scope.saveItem = function(item){
                if (item.id == undefined){
                    item.productId = $scope.selectedProduct.id;
                    item.invoiceId = $scope.invoice.id;
                    DataService.insert("items", item, function(){
                        DataService.read("invoices", $scope.invoice.id, function(data){
                            $scope.invoice = data;
                            $scope.newItem = { productId: 0, quantity: 0, price: 0 };
                            $scope.selectedProduct = { id: 0, name: "" };
                        })
                    });
                }
                else{
                    DataService.update("items", item.id, item, function(){});
                }
            };
            
        $scope.removeItem = function (item) {
            DataService.delete("items", item.id, function () {
                DataService.read("invoices", $scope.invoice.id, function (data) { $scope.invoice = data; })
            });
        }

        //LIST/GET ALL SHIPPERS
        function getShippers(name) {
            DataService.list("shippers/" + name, function(data) { $scope.shippers = data });
        };

        //LIST/GET ALL AGENTS
        function getAgents(name) {
            DataService.list("agents", function(data) { $scope.agents = data });
        };

    }]);

}());