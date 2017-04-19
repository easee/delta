(function() {
    app.controller("InvoicesCtrl", ['$scope', 'DataService', '$http', function($scope, DataService, $http) {
        $scope.showInvoice = false;
        getShippers('');
        getAgents('');
        // getCustomers('');
        ListInvoices();
        //getProduct();

        $scope.selectedCustomer = { id: 0, name: "" };


        //READ AND EDIT INVOICES
        $scope.edit = function(currentInvoice) {
            $scope.invoice = currentInvoice;
            $scope.showInvoice = true;
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
                id: 0,
                invoiceNo: invGenNum,
                date: new Date(),
                shippedOn: dateShipped,
                status: 0,
                subTotal: 0,
                vat: 0,
                vatAmount: 0,
                total: 0,
                shipperId: 0,
                agentId: 0,
                customerId: 0,
                shipping: 0,
                //START OF ITEM SECTION IN MODAL
                items: []
            };
            $scope.showInvoices = true;

            $scope.add = function() {
                $scope.invoice.items.push({
                    productId: 0,
                    quantity: 0,
                    price: 0,
                    invoiceId: 0,
                });
            };

            $scope.remove = function(index) {
                    $scope.invoice.items.splice(index, 1);
                },

                $scope.total = function() {
                    var total = 0;
                    angular.forEach($scope.invoice.items, function(item) {
                        total += item.quantity * item.price;
                    })
                    return total;
                }

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

        $scope.getProduct = function(name) {
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


        //LIST/GET ALL ITEMS
        function getItems(name) {
            DataService.list("items/" + name, function(data) { $scope.items = data });
        };

        //LIST/GET ALL SHIPPERS
        function getShippers(name) {
            DataService.list("shippers/" + name, function(data) { $scope.shippers = data });
        };

        //LIST/GET ALL AGENTS
        function getAgents(name) {
            DataService.list("agents", function(data) { $scope.agents = data });
        };

        // //LIST/GET ALL CUSTOMERS
        // function getCustomers(name){
        //     DataService.list("customers/" + name, function(data){ $scope.customers = data});
        // };
    }]);

}());