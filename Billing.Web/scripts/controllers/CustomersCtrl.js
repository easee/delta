(function(){
    app.controller("CustomersCtrl", ['$scope', 'DataService',  function($scope, DataService) {
        $scope.showCustomer = false;
        ListCustomers();

        //READ AND EDIT CUSTOMERS
        $scope.edit = function(currentCustomer){
            $scope.customer = currentCustomer;
            $scope.showCustomer = true;
        };

        //UPDATE CUSTOMER
        $scope.save = function(){
            if($scope.customer.id == 0)
                DataService.insert("customers", $scope.customer, function(data){ ListCustomers();} );
            else
                DataService.update("customers", $scope.customer.id, $scope.customer, function(data){ListCustomers();});
        };

        //CREATE NEW CUSTOMER
        $scope.new = function(){
            $scope.customer = {
                id: 0,
                name: "",
                address: "",
                town: ""
            };

//            DataService.insert("customers", $scope.customer, function(data){ ListCustomers();} ); //Ovo sam ja dodao u nadi da proradi insert sa gornje update funkcije, jer gore radi ok.
            $scope.showCustomer = true;
        };
        
        //DELETE CUSTOMER
        $scope.delete = function (customer) {
            DataService.delete("customers", customer.id, function (data) {
                ListCustomers();
            });
            $scope.showCustomers = false;
        };
        //LIST ALL CUSTOMERS
        function ListCustomers(){
            DataService.list("customers", function(data){ $scope.customers = data});
        }
    }]);

}());